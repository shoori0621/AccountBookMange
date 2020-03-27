using DialogService.Views;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogService
{
    public static class DialogServiceExtensions
    {
        /// <summary>
        /// OKダイアログを表示
        /// </summary>
        /// <param name="dialogService"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ButtonResult ShowOKDialog(this IDialogService dialogService, string message)
        {
            ButtonResult dialogResult = ButtonResult.Cancel;
            dialogService.ShowDialog(nameof(OKDialog), new DialogParameters($"Message={message}"), ret => dialogResult = ret.Result);

            return dialogResult;
        }

        /// <summary>
        /// YesNoダイアログを表示
        /// </summary>
        /// <param name="dialogService"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ButtonResult ShowYesNoDialog(this IDialogService dialogService, string message)
        {
            ButtonResult dialogResult = ButtonResult.Cancel;
            dialogService.ShowDialog(nameof(YesNoDialog), new DialogParameters($"Message={message}"), ret => dialogResult = ret.Result);

            return dialogResult;
        }
    }
}
