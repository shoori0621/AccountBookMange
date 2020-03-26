using DatabaseProvidor.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Reactive.Bindings;
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

        /// <summary>入金一覧</summary>
        public ReactiveCollection<Income> Incomes { get; }

        /// <summary>口座一覧</summary>
        public ReactiveCollection<Account> Accounts { get; }

        /// <summary>ユーザ</summary>
        private User User { get; set; }

        /// <summary>ReactivePropertyのDispose用リスト</summary>
        private System.Reactive.Disposables.CompositeDisposable disposables
            = new System.Reactive.Disposables.CompositeDisposable();

        public IncomeEditorViewModel()
        {
            this.IncomeDate = new ReactiveProperty<DateTime>();
            this.IncomeKind = new ReactiveProperty<long>();
            this.IncomePrice = new ReactiveProperty<long?>();
            this.AccountId = new ReactiveProperty<long>();
            this.Accounts = new ReactiveCollection<Account>();
        }

        void System.IDisposable.Dispose() { this.disposables.Dispose(); }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            this.User = new User((long)navigationContext.Parameters["UserId"]);

            this.Accounts.AddRangeOnScheduler(this.User.Accounts);
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

            this.IncomePrice.Value = null;
            this.IncomeDate.Value = DateTime.Now.Date;
            this.IncomeKind.Value = 0;
            this.AccountId.Value = 1;
        }
    }
}
