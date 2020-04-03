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
    public class LoginMenuViewModel : BindableBase, IConfirmNavigationRequest, IDisposable
    {
        /// <summary>入金額</summary>
        public ReactiveProperty<string> Account { get; set; }

        /// <summary>入金額</summary>
        public ReactiveProperty<string> Password { get; set; }
        
        /// <summary>ログインコマンド</summary>
        public ReactiveCommand LoginCommand { get; }

        /// <summary>ユーザ</summary>
        private User User { get; set; }

        private IDialogService dialogService;

        /// <summary>ReactivePropertyのDispose用リスト</summary>
        private System.Reactive.Disposables.CompositeDisposable disposables
            = new System.Reactive.Disposables.CompositeDisposable();

        public LoginMenuViewModel(IDialogService dialogService)
        {
            this.Account = new ReactiveProperty<string>();
            this.Password = new ReactiveProperty<string>();

            this.LoginCommand = new ReactiveCommand().AddTo(this.disposables);
            this.LoginCommand.Subscribe(() => this.Login());

            this.dialogService = dialogService;
        }

        void System.IDisposable.Dispose() { this.disposables.Dispose(); }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            this.Initialize();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            this.Initialize();
        }

        public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 項目を初期化する
        /// </summary>
        private void Initialize()
        {
            this.User = new User();
            this.Account.Value = null;
            this.Password.Value = null;
        }

        private void Login()
        {
            this.User = DatabaseProvidor.Models.User.Login(this.Account.Value, this.Password.Value);

            if(this.User.Id == 0)
            {
                DialogServiceExtensions.ShowOKDialog(this.dialogService, "アカウント、又はパスワードに誤りがあります");
                return;
            }


        }
    }
}
