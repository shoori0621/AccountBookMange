using DatabaseProvidor.Models;
using DialogService;
using DialogService.Views;
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
    public class IncomeEditorViewModel : BindableBase, INavigationAware, IDisposable
    {
        /// <summary>入金額</summary>
        public ReactiveProperty<long?> IncomePrice { get; set; }

        /// <summary>入金額</summary>
        public ReactiveProperty<DateTime> IncomeDate { get; set; }

        /// <summary>入金区分</summary>
        public ReactiveProperty<long> IncomeKind { get; set; }

        /// <summary>口座</summary>
        public ReactiveProperty<long> AccountId { get; set; }

        /// <summary>備考</summary>
        public ReactiveProperty<string> Comment { get; set; }

        /// <summary>選択した収入</summary>
        public ReactiveProperty<Income> SelectedIncome { get; set; }

        /// <summary>入金一覧</summary>
        public ReactiveCollection<Income> Incomes { get; } = new ReactiveCollection<Income>();

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

        public IncomeEditorViewModel(IDialogService dialogService)
        {
            this.IncomeDate = new ReactiveProperty<DateTime>();
            this.IncomeKind = new ReactiveProperty<long>();
            this.IncomePrice = new ReactiveProperty<long?>();
            this.AccountId = new ReactiveProperty<long>();
            this.Accounts = new ReactiveCollection<Account>();
            this.Comment = new ReactiveProperty<string>();
            this.SelectedIncome = new ReactiveProperty<Income>();

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
            this.Incomes.Clear();
            this.Accounts.Clear();

            foreach(var income in this.User.Incomes)
            {
                income.DeleteCommand = new DelegateCommand(() => { this.Delete(); });
                this.Incomes.Add(income);
            }

            this.Accounts.AddRangeOnScheduler(this.User.Accounts);

            this.IncomePrice.Value = null;
            this.IncomeDate.Value = DateTime.Now.Date;
            this.IncomeKind.Value = 0;
            this.AccountId.Value = 0;
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

            if(dialogResult == ButtonResult.No)
            {
                //Noの場合は処理を中断する
                return;
            }

            //入金登録処理
            var income = new Income();
            income.UserId = this.User.Id;
            income.DateTimeIncomeDate = this.IncomeDate.Value;
            income.IncomeKind = this.IncomeKind.Value;            
            income.AccountId = this.AccountId.Value;
            income.IncomePrice = this.IncomePrice.Value;
            income.Comment = this.Comment.Value;

            income.Upsert();

            //初期化する  
            this.IncomePrice.Value = null;
            this.IncomeDate.Value = DateTime.Now.Date;
            this.IncomeKind.Value = 0;
            this.AccountId.Value = 0;
            this.Comment.Value = null;
            this.Incomes.Insert(0, income);

        }

        /// <summary>
        /// 入力チェック
        /// </summary>
        /// <returns></returns>
        private bool CheckRegistration()
        {
            //入力誤りが無いかチェックする
            bool result = true;
            if (this.IncomePrice.HasErrors)
            {
                result = false;
            }

            if (result && !this.IncomePrice.Value.HasValue)
            {
                result = false;
            }

            if (result && this.IncomeDate.HasErrors)
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
        private void Delete()
        {
            var result = DialogServiceExtensions.ShowYesNoDialog(this.dialogService, "削除します\nよろしいですか?");

            if(result == ButtonResult.No)
            {
                return;
            }

            this.SelectedIncome.Value.Delete();
        }
    }
}
