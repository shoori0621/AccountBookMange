using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditorViews.Constants
{
    public static class ListDefine
    {
        /// <summary>
        /// コンボボックス用来院期間クラス
        /// </summary>
        public class DefineItem
        {
            public string content { get; set; }
            public long? value { get; set; }
        }

        /// <summary>
        /// 入金区分
        /// </summary>
        public static DefineItem[] IncomeKinds = new DefineItem[] {
            new DefineItem { value = 0,    content = "給与"},
            new DefineItem { value = 1,    content = "賞与"},
            new DefineItem { value = 2,    content = "臨時収入"},
            new DefineItem { value = 9,    content = "その他"},
        };

        /// <summary>
        /// 支払区分
        /// </summary>
        public static DefineItem[] PaymentKinds = new DefineItem[] {
            new DefineItem { value = 0,    content = "生活必需品"},
            new DefineItem { value = 1,    content = "食費"},
            new DefineItem { value = 2,    content = "趣味"},
            new DefineItem { value = 3,    content = "交際費"},
            new DefineItem { value = 9,    content = "その他"},
        };

        /// <summary>
        /// 支払方法
        /// </summary>
        public static DefineItem[] PaymentWays = new DefineItem[] {
            new DefineItem { value = 0,    content = "現金"},
            new DefineItem { value = 1,    content = "クレジットカード"},
            new DefineItem { value = 2,    content = "銀行"},
            new DefineItem { value = 3,    content = "電子マネー"},
        };
    }
}
