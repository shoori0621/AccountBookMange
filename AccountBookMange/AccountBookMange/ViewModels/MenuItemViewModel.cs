using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace AccountBookMange.ViewModels
{
    public class MenuItemViewModel : BindableBase, IDisposable
    {
        /// <summary>メニューに表示されるアイコン</summary>
        public ReadOnlyReactivePropertySlim<string> Icon { get; }

        /// <summary>メニュー名</summary>
        public ReadOnlyReactiveProperty<string> MenuTitle { get; }

        /// <summary>メニューの子要素</summary>
        public ReactiveCollection<MenuItemViewModel> Children { get; }

        /// <summary>TreeViewItemが展開されているかを取得・設定</summary>
        public ReactivePropertySlim<bool> IsExpanded { get; set; }

        /// <summary>TreeViewItemが展開されているかを取得・設定</summary>
        public ReactivePropertySlim<bool> IsSelected { get; set; }

        /// <summary>ReactivePropertyのDispose用リスト</summary>
        private System.Reactive.Disposables.CompositeDisposable disposables
            = new System.Reactive.Disposables.CompositeDisposable();
    }
}
