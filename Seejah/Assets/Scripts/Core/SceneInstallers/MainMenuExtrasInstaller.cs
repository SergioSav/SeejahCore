using Assets.Scripts.Core.Commands;
using Assets.Scripts.Core.HUD;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Assets.Scripts.Core.SceneInstallers
{
    public class MainMenuExtrasInstaller : IInstaller
    {
        public void Install(IContainerBuilder builder)
        {
            builder.Register<ISelectCustomizationItemCommand, SelectCustomizationItemCommand>(Lifetime.Singleton);
            builder.RegisterFactory<CustomizationItemPresenter, Transform, CustomizationItemPresenter>(container =>
            {
                return (prefab, parentTransform) => container.Instantiate(prefab, parentTransform);
            },
            Lifetime.Singleton);

            builder.RegisterComponentInHierarchy<MainMenuPresenter>();
            builder.RegisterComponentInHierarchy<CustomizationMenuPresenter>();
        }
    }
}
