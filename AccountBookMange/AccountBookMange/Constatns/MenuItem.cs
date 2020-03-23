using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountBookMange.Constatns
{
    public enum MenuItem
    {
        None = 0,
        /// <summary>メインメニュー</summary>
        MainMenu = 1,
        /// <summary>入金</summary>
        Income = 2,
        /// <summary>支払</summary>
        Payment = 3,
        /// <summary>移動</summary>
        Move = 4,
        /// <summary>ユーザ設定</summary>
        UserSetting = 5,
        /// <summary>ユーザ情報</summary>
        User = 6,
        /// <summary>各種設定</summary>
        MasterMenu = 7,
        /// <summary>ユーザ管理</summary>
        UserMaster = 8
    }
}
