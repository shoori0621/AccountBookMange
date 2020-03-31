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
    public class PaymentEditorViewModel : BindableBase, INavigationAware, IDisposable
    {
        /// <summary>支払額</summary>
        public ReactiveProperty<long?> PaymentPrice { get; set; }

        /// <summary>支払額</summary>
        public ReactiveProperty<DateTime> PaymentDate { get; set; }

        /// <summary>支払区分</summary>
        public ReactiveProperty<long> PaymentKind { get; set; }

        /// <summary>支払い方法</summary>
        public ReactiveProperty<long> PaymentWay { get; set; }

        /// <summary>口座</summary>
        public ReactiveProperty<long> AccountId { get; set; }

        /// <summary>備考</summary>
        public ReactiveProperty<string> Comment { get; set; }

        /// <summary>支払一覧</summary>
        public ReactiveCollection<Payment> Payments { get; } = new ReactiveCollection<Payment>();

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

        public PaymentEditorViewModel(IDialogService dialogService)
        {
            this.PaymentDate = new ReactiveProperty<DateTime>();
            this.PaymentKind = new ReactiveProperty<long>();
            this.PaymentWay = new ReactiveProperty<long>();
            this.PaymentPrice = new ReactiveProperty<long?>();
            this.AccountId = new ReactiveProperty<long>();
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
            this.Payments.Clear();
            this.Accounts.Clear();

            foreach (var payment in this.User.Payments)
            {
                payment.DeleteCommand = new DelegateCommand(() => { this.Delete(payment); });
                this.Payments.Add(payment);
            }

            this.Accounts.AddRangeOnScheduler(this.User.Accounts);

            this.PaymentPrice.Value = null;
            this.PaymentDate.Value = DateTime.Now.Date;
            this.PaymentKind.Value = 0;
            this.AccountId.Value = 1;
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

            //支払登録処理
            var payment = new Payment();
            payment.DeleteCommand = new DelegateCommand(() => { this.Delete(payment); });
            payment.UserId = this.User.Id;
            payment.DateTimePaymentDate = this.PaymentDate.Value;
            payment.PaymentKind = this.PaymentKind.Value;
            payment.PaymentWay = this.PaymentWay.Value;
            payment.AccountId = this.AccountId.Value;
            payment.PaymentPrice = this.PaymentPrice.Value;
            payment.Comment = this.Comment.Value;

            payment.Upsert();

            //初期化する  
            this.PaymentPrice.Value = null;
            this.PaymentDate.Value = DateTime.Now.Date;
            this.PaymentKind.Value = 0;
            this.AccountId.Value = 1;
            this.Comment.Value = null;
            this.Payments.Insert(0, payment);

        }

        /// <summary>
        /// 入力チェック
        /// </summary>
        /// <returns></returns>
        private bool CheckRegistration()
        {
            //入力誤りが無いかチェックする
            bool result = true;
            if (this.PaymentPrice.HasErrors)
            {
                result = false;
            }

            if (result && !this.PaymentPrice.Value.HasValue)
            {
                result = false;
            }

            if (result && this.PaymentDate.HasErrors)
            {
                result = false;
            }

            if (result && this.AccountId.Value == 0)
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
        private void Delete(Payment payment)
        {
            var result = DialogServiceExtensions.ShowYesNoDialog(this.dialogService, "削除します\nよろしいですか?");

            if (result == ButtonResult.No)
            {
                return;
            }

            payment.Delete();
            this.Payments.Remove(payment);
        }
    }
}
