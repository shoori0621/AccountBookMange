using AccountBookMange.Constatns;
using DatabaseProvidor.Models;

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
        public ReactiveProperty<string> Icon { get; }

        /// <summary>メニュー名</summary>
        public ReactiveProperty<string> MenuTitle { get; }

        /// <summary>メニューの子要素</summary>
        public ReactiveCollection<MenuItemViewModel> Children { get; }

        /// <summary>TreeViewItemが展開されているかを取得・設定</summary>
        public ReactivePropertySlim<bool> IsExpanded { get; set; }

        /// <summary>TreeViewItemが展開されているかを取得・設定</summary>
        public ReactivePropertySlim<bool> IsSelected { get; set; }

        /// <summary>メニューの種類</summary>
        public ReactiveProperty<MenuItem> MenuItemType { get; }

        /// <summary>ReactivePropertyのDispose用リスト</summary>
        private System.Reactive.Disposables.CompositeDisposable disposables
            = new System.Reactive.Disposables.CompositeDisposable();
        
        public MenuItemViewModel(MenuItem menuItem)
        {
            this.Children = new ReactiveCollection<MenuItemViewModel>().AddTo(this.disposables);

            this.MenuItemType = new ReactiveProperty<MenuItem>(menuItem); 

            switch (this.MenuItemType.Value)
            {
                case MenuItem.MainMenu:
                    //メインメニュー
                    this.MenuTitle = new ReactiveProperty<string>("ユーザメニュー");
                    this.Icon = new ReactiveProperty<string>("BarChart");
                    break;
                case MenuItem.Income:
                    //入金
                    this.MenuTitle = new ReactiveProperty<string>("収入登録・変更");
                    this.Icon = new ReactiveProperty<string>("AngleDoubleLeft");
                    break;
                case MenuItem.Payment:
                    //支払
                    this.MenuTitle = new ReactiveProperty<string>("支出登録・変更");
                    this.Icon = new ReactiveProperty<string>("AngleDoubleRight");
                    break;
                case MenuItem.Move:
                    //支払
                    this.MenuTitle = new ReactiveProperty<string>("移動登録・変更");
                    this.Icon = new ReactiveProperty<string>("Exchange");
                    break;
                case MenuItem.UserSetting:
                    //設定
                    this.MenuTitle = new ReactiveProperty<string>("設定");
                    this.Icon = new ReactiveProperty<string>("Gear");
                    break;
                case MenuItem.User:
                    //ユーザ情報
                    this.MenuTitle = new ReactiveProperty<string>("ユーザ情報");
                    this.Icon = new ReactiveProperty<string>("AddressCardOutline");
                    break;
                case MenuItem.MasterMenu:
                    //マスタメニュー
                    this.MenuTitle = new ReactiveProperty<string>("マスタメニュー");
                    this.Icon = new ReactiveProperty<string>("Gears");
                    break;
                case MenuItem.UserMaster:
                    //ユーザマスタ
                    this.MenuTitle = new ReactiveProperty<string>("ユーザ管理");
                    this.Icon = new ReactiveProperty<string>("Users");
                    break;
                default:
                    this.MenuTitle = new ReactiveProperty<string>("家計簿");
                    this.Icon = new ReactiveProperty<string>("Yen");
                    break;
            }

            this.IsExpanded = new ReactivePropertySlim<bool>(true).AddTo(this.disposables);
            this.IsSelected = new ReactivePropertySlim<bool>(true).AddTo(this.disposables);
        }

        /// <summary>オブジェクトを破棄します。</summary>
        void IDisposable.Dispose() { this.disposables.Dispose(); }
    }
}
