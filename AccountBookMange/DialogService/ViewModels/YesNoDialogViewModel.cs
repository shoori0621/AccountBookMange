using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DialogService.ViewModels
{
    public class YesNoDialogViewModel : BindableBase, IDialogAware
    {
        /// <summary>メッセージボックスのタイトルを取得します。</summary>
        public string Title => "メッセージボックスTEST";

        /// <summary>メッセージボックスへ表示する文字列を取得します。</summary>
        public ReactivePropertySlim<string> Message { get; }

        /// <summary>はいボタンのCommandを取得します。</summary>
        public ReactiveCommand YesCommand { get; } = new ReactiveCommand();

        /// <summary>いいえボタンのCommandを取得します。</summary>
        public ReactiveCommand NoCommand { get; } = new ReactiveCommand();

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

        public YesNoDialogViewModel()
        {
            this.Message = new ReactivePropertySlim<string>(string.Empty);

            this.YesCommand.Subscribe(() => this.RequestClose?.Invoke(new DialogResult(ButtonResult.Yes)));
            this.NoCommand.Subscribe(() => this.RequestClose?.Invoke(new DialogResult(ButtonResult.No)));
        }
    }
}
