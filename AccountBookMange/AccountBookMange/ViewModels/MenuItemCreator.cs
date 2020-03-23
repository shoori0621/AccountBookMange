using AccountBookMange.Constatns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountBookMange.ViewModels
{
    internal static class MenuItemCreator
    {
        internal static MenuItemViewModel Careate()
        {
            var root = new MenuItemViewModel(MenuItem.None);

            //メインメニュー
            var main = new MenuItemViewModel(MenuItem.MainMenu);

            //収入登録・変更
            main.Children.Add(new MenuItemViewModel(MenuItem.Income));

            //支出登録・変更
            main.Children.Add(new MenuItemViewModel(MenuItem.Payment));

            //移動登録・変更
            main.Children.Add(new MenuItemViewModel(MenuItem.Move));

            //ユーザ設定
            var userSetting = new MenuItemViewModel(MenuItem.UserSetting);

            //ユーザ情報
            userSetting.Children.Add(new MenuItemViewModel(MenuItem.UserSetting));

            //マスタメニュー
            var userMaster = new MenuItemViewModel(MenuItem.MasterMenu);

            //ユーザマスタ
            userMaster.Children.Add(new MenuItemViewModel(MenuItem.UserMaster));

            root.Children.Add(main);
            root.Children.Add(userMaster);

            return root;
        }
    }
}
