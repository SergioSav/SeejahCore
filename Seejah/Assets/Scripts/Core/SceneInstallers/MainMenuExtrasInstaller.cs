using Assets.Scripts.Core.HUD;
using VContainer;
using VContainer.Unity;

namespace Assets.Scripts.Core.SceneInstallers
{
    public class MainMenuExtrasInstaller : IInstaller
    {
        public void Install(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<MainMenuPresenter>();
            builder.RegisterComponentInHierarchy<CustomizationMenuPresenter>();
        }
    }
}
