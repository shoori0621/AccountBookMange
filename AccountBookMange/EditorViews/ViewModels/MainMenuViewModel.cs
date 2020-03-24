using Prism.Commands;
using Prism.Mvvm;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;

using DatabaseProvidor.Models;
using Prism.Regions;

namespace EditorViews.ViewModels
{
    public class MainMenuViewModel : BindableBase, INavigationAware, IDisposable
    {
        /// <summary>貯蓄額</summary>
        public ReactiveProperty<long> SavingPrice { get; set; }

        /// <summary>前月比</summary>
        public ReactiveProperty<long> LastMonthDiffPrice { get; set; }

        /// <summary>前年比</summary>
        public ReactiveProperty<long> LastYearDiffPrice { get; set; }

        /// <summary>収入履歴</summary>
        public ReactiveCollection<Income> Incomes { get; }

        /// <summary>支出履歴</summary>
        public ReactiveCollection<Payment> Payments { get; }

        /// <summary>移動履歴</summary>
        public ReactiveCollection<Move> Moves { get; }

        private User User { get; set; }

        /// <summary>ReactivePropertyのDispose用リスト</summary>
        private System.Reactive.Disposables.CompositeDisposable disposables
            = new System.Reactive.Disposables.CompositeDisposable();

        void System.IDisposable.Dispose() { this.disposables.Dispose(); }

        public MainMenuViewModel()
        {
            this.SavingPrice = new ReactiveProperty<long>();
            this.LastMonthDiffPrice = new ReactiveProperty<long>();
            this.LastYearDiffPrice = new ReactiveProperty<long>();
            this.Incomes = new ReactiveCollection<Income>();
            this.Payments = new ReactiveCollection<Payment>();
            this.Moves = new ReactiveCollection<Move>();
        }
                
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            this.User = new User((long)navigationContext.Parameters["UserId"]);

            this.SetContent();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            this.User = new User((long)navigationContext.Parameters["UserId"]);
        }

        /// <summary>
        /// 画面へ表示する
        /// </summary>
        private void SetContent()
        {
            //収入、支払、移動を初期化
            this.Incomes.Clear();
            this.Payments.Clear();
            this.Moves.Clear();

            //当日を取得
            var now = DateTime.Now.Date;

            //this.SavingPrice = new ReactiveProperty<long>(0);
            this.SavingPrice.Value = 0;
            this.LastMonthDiffPrice.Value = 0;
            this.LastYearDiffPrice.Value = 0;

            //var incomes = Income.FindByUserId(this.UserId);
            //var payments = Payment.FindByUserId(this.UserId);
            //var moves = Move.FindByUserId(this.UserId);

            long preMonthSavingPrice = 0;
            long preYearSavingPrice = 0;

            //収入の計算
            foreach (var income in this.User.Incomes)
            {

                if (now >= income.DateTimeIncomeDate)
                {
                    //入金日を迎えていれば加算する
                    this.SavingPrice.Value += (income.IncomePrice ?? 0);
                }

                //前月比の計算
                //入金日の年+月
                int incomeYearMonth = income.DateTimeIncomeDate.Value.Year + income.DateTimeIncomeDate.Value.Month;
                //当日の年+月
                int nowYearMonth = now.Year + now.Month;

                //前月以前であれば前月までの収入として加算
                if (incomeYearMonth < nowYearMonth)
                {
                    preMonthSavingPrice += (income.IncomePrice ?? 0);
                }

                //前年以前であれば前年までの収入として加算
                if (income.DateTimeIncomeDate.Value.Year < now.Year)
                {
                    preYearSavingPrice += (income.IncomePrice ?? 0);
                }

                this.Incomes.AddOnScheduler(income);
            }

            //支出の計算
            foreach (var payment in this.User.Payments)
            {
                if (now >= payment.DateTimePaymentDate)
                {
                    //支払日を迎えていれば差し引く
                    this.SavingPrice.Value -= (payment.PaymentPrice ?? 0);
                }

                //前月比の計算
                //入金日の年+月
                int incomeYearMonth = payment.DateTimePaymentDate.Value.Year + payment.DateTimePaymentDate.Value.Month;
                //当日の年+月
                int nowYearMonth = now.Year + now.Month;

                //前月以前であれば前月までの収入として加算
                if (incomeYearMonth < nowYearMonth)
                {
                    preMonthSavingPrice -= (payment.PaymentPrice ?? 0);
                }

                //前年以前であれば前年までの収入として加算
                if (payment.DateTimePaymentDate.Value.Year < now.Year)
                {
                    preYearSavingPrice -= (payment.PaymentPrice ?? 0);
                }

                this.Payments.AddOnScheduler(payment);
            }

            //移動
            foreach (var move in this.User.Moves)
            {
                this.Moves.AddOnScheduler(move);
            }

            //前月比
            this.LastMonthDiffPrice.Value = this.SavingPrice.Value - preMonthSavingPrice;
            //前年比
            this.LastYearDiffPrice.Value = this.SavingPrice.Value - preYearSavingPrice;
        }
    }
}
