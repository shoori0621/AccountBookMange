using Prism.Mvvm;
using Prism.Services.Dialogs;
using Reactive.Bindings;
using System;
using System.ComponentModel;

namespace DialogService.ViewModels
{
    public class OKDialogViewModel : BindableBase, IDialogAware
    {
        /// <summary>メッセージボックスのタイトルを取得します。</summary>
        public string Title => "メッセージボックスTEST";

        /// <summary>メッセージボックスへ表示する文字列を取得します。</summary>
        public ReactivePropertySlim<string> Message { get; }

        /// <summary>OKボタンのCommandを取得します。</summary>
        public ReactiveCommand OkCommand { get; } = new ReactiveCommand();
        
        /// <summary>ダイアログのCloseを要求するAction。</summary>
        public event Action<IDialogResult> RequestClose;

        /// <summary>ダイアログがClose可能かを取得します。</summary>
        /// <returns></returns>
        public bool CanCloseDialog() { return true; }

        /// <summary>ダイアログClose時のイベントハンドラ。</summary>
        public void OnDialogClosed() { }

        /// <summary>ダイアログOpen時のイベントハンドラ。</summary>
        /// <param name="parameters">IDialogServiceに設定されたパラメータを表すIDialogParameters。</param>
        public void OnDialogOpened(IDialogParameters parameters)
        {
            this.Message.Value = parameters.GetValue<string>("Message");
        }

        public OKDialogViewModel()
        {
            this.Message = new ReactivePropertySlim<string>(string.Empty);

            this.OkCommand.Subscribe(() => this.RequestClose?.Invoke(new DialogResult(ButtonResult.OK)));
        }
    }
}
