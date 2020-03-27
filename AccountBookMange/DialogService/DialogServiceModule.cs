using DialogService.ViewModels;
using DialogService.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace DialogService
{
    public class DialogServiceModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
 
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<OKDialog, OKDialogViewModel>();
            containerRegistry.RegisterDialog<YesNoDialog, YesNoDialogViewModel>();
        }
    }
}