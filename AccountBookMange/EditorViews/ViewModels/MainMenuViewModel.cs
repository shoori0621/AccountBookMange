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

        /// <summary>ReactivePropertyのDispose用リスト</summary>
        private System.Reactive.Disposables.CompositeDisposable disposables
            = new System.Reactive.Disposables.CompositeDisposable();

        public MainMenuViewModel()
        {
            //当日を取得
            var now = DateTime.Now.Date;

            this.SavingPrice = new ReactiveProperty<long>(0);
            this.LastMonthDiffPrice = new ReactiveProperty<long>(0);
            this.LastYearDiffPrice = new ReactiveProperty<long>(0);

            var incomes = Income.FindByUserId(1);
            var payments = Payment.FindByUserId(1);
            var moves = Move.FindByUserId(1);

            long preMonthSavingPrice = 0;
            long preYearSavingPrice = 0;

            //収入の計算
            foreach(var income in incomes)
            {
                //入金日を日付型に変換
                DateTime incomeDate = DateTime.ParseExact(income.IncomeDate, "yyyy/MM/dd", null);

                if(now >= incomeDate)
                {
                    //入金日を迎えていれば加算する
                    this.SavingPrice.Value += (income.IncomePrice ?? 0);
                }

                //前月比の計算
                //入金日の年+月
                int incomeYearMonth = incomeDate.Year + incomeDate.Month;
                //当日の年+月
                int nowYearMonth = now.Year + now.Month;

                //前月以前であれば前月までの収入として加算
                if (incomeYearMonth < nowYearMonth)
                {
                    preMonthSavingPrice += (income.IncomePrice ?? 0);
                }

                //前年以前であれば前年までの収入として加算
                if(incomeDate.Year < now.Year)
                {
                    preYearSavingPrice += (income.IncomePrice ?? 0);
                }

                this.Incomes.Add(income);
            }

            //支出の計算
            foreach (var payment in payments)
            {
                //支払日を日付型に変換
                DateTime paymentDate = DateTime.ParseExact(payment.PaymentDate, "yyyy/MM/dd", null);

                if(now >= paymentDate)
                {
                    //支払日を迎えていれば差し引く
                    this.SavingPrice.Value -= (payment.PaymentPrice ?? 0);
                }

                //前月比の計算
                //入金日の年+月
                int incomeYearMonth = paymentDate.Year + paymentDate.Month;
                //当日の年+月
                int nowYearMonth = now.Year + now.Month;

                //前月以前であれば前月までの収入として加算
                if (incomeYearMonth < nowYearMonth)
                {
                    preMonthSavingPrice -= (payment.PaymentPrice ?? 0);
                }

                //前年以前であれば前年までの収入として加算
                if (paymentDate.Year < now.Year)
                {
                    preYearSavingPrice -= (payment.PaymentPrice ?? 0);
                }

                this.Payments.Add(payment);
            }

            //移動
            foreach (var move in moves)
            {
                this.Moves.Add(move);
            }

            //前月比
            this.LastMonthDiffPrice.Value = this.SavingPrice.Value - preMonthSavingPrice;
            //前年比
            this.LastYearDiffPrice.Value = this.SavingPrice.Value - preYearSavingPrice;

        }

        void System.IDisposable.Dispose() { this.disposables.Dispose(); }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            throw new NotImplementedException();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            throw new NotImplementedException();
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            throw new NotImplementedException();
        }
    }
}
