using EditorViews.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace EditorViews
{
    public class EditorViewsModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
 
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<IncomeEditor>();
            containerRegistry.RegisterForNavigation<LoginMenu>();
            containerRegistry.RegisterForNavigation<MainMenu>();
            containerRegistry.RegisterForNavigation<MoveEditor>();
            containerRegistry.RegisterForNavigation<PaymentEditor>();
            containerRegistry.RegisterForNavigation<UserEditor>();
            containerRegistry.RegisterForNavigation<UserMaster>();
        }
    }
}