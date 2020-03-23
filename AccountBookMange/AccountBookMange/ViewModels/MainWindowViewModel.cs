using Prism.Mvvm;
using Prism.Regions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System.Collections.ObjectModel;
using System.Windows;

using AccountBookMange.Constatns;

namespace AccountBookMange.ViewModels
{
    public class MainWindowViewModel : BindableBase, System.IDisposable
    {
        private string _title = "家計簿";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        /// <summary>TreeViewItem を取得します。</summary>
		public ReadOnlyReactiveCollection<MenuItemViewModel> MenuItems { get; }

        private IRegionManager regionManager = null;

        private MenuItemViewModel rootNode;

        /// <summary>ReactivePropertyのDispose用リスト</summary>
        private System.Reactive.Disposables.CompositeDisposable disposables
            = new System.Reactive.Disposables.CompositeDisposable();

        /// <summary>ロードイベントコマンド</summary>
        public ReactiveCommand Loaded { get; }

        /// <summary>メニュー変更イベント</summary>
        public ReactiveCommand<RoutedPropertyChangedEventArgs<object>> SelectedItemChanged { get;}

        public MainWindowViewModel(IRegionManager rm)
        {
            this.regionManager = rm;

            //メニューを生成
            this.rootNode = MenuItemCreator.Careate();

            var col = new ObservableCollection<MenuItemViewModel>();
            col.Add(rootNode);

            this.MenuItems = col.ToReadOnlyReactiveCollection().AddTo(this.disposables);

            this.Loaded = new ReactiveCommand().AddTo(this.disposables);
            this.Loaded.Subscribe(() => this.rootNode.IsSelected.Value = true);

            this.SelectedItemChanged = new ReactiveCommand<RoutedPropertyChangedEventArgs<object>>()
                .WithSubscribe(e => this.SelectedMenuChanged(e))
                .AddTo(this.disposables);
        }

        private void SelectedMenuChanged(RoutedPropertyChangedEventArgs<object> e)
        {
            string viewName = "";
            var current = e.NewValue as MenuItemViewModel;

            switch (current.MenuItemType.Value)
            {
                case MenuItem.MainMenu:
                    //メインメニュー
                    viewName = "MainMenu";
                    break;
                case MenuItem.Income:
                    //入金
                    viewName = "IncomeEditor";
                    break;
                case MenuItem.Payment:
                    //支払
                    viewName = "PaymentEditor";
                    break;
                case MenuItem.Move:
                    //支払
                    viewName = "MoveEditor";
                    break;
                case MenuItem.UserSetting:
                    //設定
                    viewName = "";
                    break;
                case MenuItem.User:
                    //ユーザ情報
                    viewName = "UserEditor";
                    break;
                case MenuItem.MasterMenu:
                    //マスタメニュー
                    viewName = "";
                    break;
                case MenuItem.UserMaster:
                    //ユーザマスタ
                    viewName = "UserMaster";
                    break;
                default:
                    viewName = "";
                    break;
            }

            this.regionManager.RequestNavigate("EditorArea", viewName);
            
        }

        void System.IDisposable.Dispose() { this.disposables.Dispose(); }
    }
}
