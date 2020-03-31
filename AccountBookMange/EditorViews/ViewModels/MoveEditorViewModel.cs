using DatabaseProvidor.Models;
using DialogService;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EditorViews.ViewModels
{
    public class MoveEditorViewModel : BindableBase, INavigationAware, IDisposable
    {
        /// <summary>金額</summary>
        public ReactiveProperty<long?> MovePrice { get; set; }

        /// <summary>移動開始日</summary>
        public ReactiveProperty<DateTime> StartDate { get; set; }

        /// <summary>移動完了日</summary>
        public ReactiveProperty<DateTime> EndDate { get; set; }

        /// <summary>移動元口座</summary>
        public ReactiveProperty<long> PreAccountId { get; set; }

        /// <summary>移動先口座</summary>
        public ReactiveProperty<long> NextAccountId { get; set; }

        /// <summary>備考</summary>
        public ReactiveProperty<string> Comment { get; set; }

        /// <summary>移動一覧</summary>
        public ReactiveCollection<Move> Moves { get; } = new ReactiveCollection<Move>();

        /// <summary>口座一覧</summary>
        public ReactiveCollection<Account> Accounts { get; } = new ReactiveCollection<Account>();

        /// <summary>登録コマンド</summary>
        public ReactiveCommand RegistrationCommand { get; }

        /// <summary>ユーザ</summary>
        private User User { get; set; }

        private IDialogService dialogService;

        /// <summary>ReactivePropertyのDispose用リスト</summary>
        private System.Reactive.Disposables.CompositeDisposable disposables
            = new System.Reactive.Disposables.CompositeDisposable();

        public MoveEditorViewModel(IDialogService dialogService)
        {
            this.StartDate = new ReactiveProperty<DateTime>();
            this.EndDate = new ReactiveProperty<DateTime>();            
            this.MovePrice = new ReactiveProperty<long?>();
            this.PreAccountId = new ReactiveProperty<long>();
            this.NextAccountId = new ReactiveProperty<long>();
            this.Accounts = new ReactiveCollection<Account>();
            this.Comment = new ReactiveProperty<string>();

            this.RegistrationCommand = new ReactiveCommand().AddTo(this.disposables);
            this.RegistrationCommand.Subscribe(() => this.Registration());

            this.dialogService = dialogService;
        }

        void System.IDisposable.Dispose() { this.disposables.Dispose(); }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            this.User = new User((long)navigationContext.Parameters["UserId"]);

            this.Initialize();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            this.User = new User((long)navigationContext.Parameters["UserId"]);
        }

        /// <summary>
        /// 項目を初期化する
        /// </summary>
        private void Initialize()
        {
            this.Moves.Clear();
            this.Accounts.Clear();

            foreach (var Move in this.User.Moves)
            {
                Move.DeleteCommand = new DelegateCommand(() => { this.Delete(Move); });
                this.Moves.Add(Move);
            }

            this.Accounts.AddRangeOnScheduler(this.User.Accounts);

            this.MovePrice.Value = null;
            this.StartDate.Value = DateTime.Now.Date;
            this.EndDate.Value = DateTime.Now.Date;            
            this.PreAccountId.Value = 1;
            this.NextAccountId.Value = 1;
            this.Comment.Value = null;
        }

        /// <summary>
        /// 登録処理
        /// </summary>
        private void Registration()
        {
            //入力に誤りが無いかチェック
            bool result = this.CheckRegistration();

            if (!result)
            {
                return;
            }

            //登録確認ダイアログを表示
            string message = "登録します\nよろしいですか?";
            var dialogResult = DialogServiceExtensions.ShowYesNoDialog(this.dialogService, message);

            if (dialogResult == ButtonResult.No)
            {
                //Noの場合は処理を中断する
                return;
            }

            //入金登録処理
            var move = new Move();
            move.DeleteCommand = new DelegateCommand(() => { this.Delete(move); });
            move.UserId = this.User.Id;
            move.DateTimeStartDate = this.StartDate.Value;
            move.DateTimeEndDate = this.EndDate.Value;            
            move.PreAccountId = this.PreAccountId.Value;            
            move.NextAccountId = this.NextAccountId.Value;            
            move.MovePrice = this.MovePrice.Value;
            move.Comment = this.Comment.Value;

            move.Upsert();
            move.PreAccount = new Account(this.PreAccountId.Value, this.User.Id);
            move.NextAccount = new Account(this.NextAccountId.Value, this.User.Id);

            //初期化する  
            this.MovePrice.Value = null;
            this.StartDate.Value = DateTime.Now.Date;
            this.EndDate.Value = DateTime.Now.Date;
            this.PreAccountId.Value = 1;
            this.NextAccountId.Value = 1;
            this.Comment.Value = null;
            this.Moves.Insert(0, move);

        }

        /// <summary>
        /// 入力チェック
        /// </summary>
        /// <returns></returns>
        private bool CheckRegistration()
        {
            //入力誤りが無いかチェックする
            bool result = true;
            if (this.MovePrice.HasErrors)
            {
                result = false;
            }

            if (result && !this.MovePrice.Value.HasValue)
            {
                result = false;
            }

            if (result && this.StartDate.HasErrors)
            {
                result = false;
            }

            if (result && this.EndDate.HasErrors)
            {
                result = false;
            }

            if (result && this.PreAccountId.Value == 0)
            {
                result = false;
            }

            if (result && this.NextAccountId.Value == 0)
            {
                result = false;
            }
                        
            if (!result)
            {
                DialogServiceExtensions.ShowOKDialog(this.dialogService, "入力に誤りがあります");

                return false;
            }

            return true;
        }

        /// <summary>
        /// 削除
        /// </summary>
        private void Delete(Move Move)
        {
            var result = DialogServiceExtensions.ShowYesNoDialog(this.dialogService, "削除します\nよろしいですか?");

            if (result == ButtonResult.No)
            {
                return;
            }

            Move.Delete();
            this.Moves.Remove(Move);
        }
    }
}
