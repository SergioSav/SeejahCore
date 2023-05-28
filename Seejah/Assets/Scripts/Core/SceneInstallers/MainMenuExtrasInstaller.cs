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
            builder.RegisterFactory<CustomizationItemPresenter, Transform, CustomizationItemPresenter>(container =>
            {
                return (prefab, parentTransform) => container.Instantiate(prefab, parentTransform);
            },
            Lifetime.Singleton);

            builder.RegisterComponentInHierarchy<MainMenuPresenter>();
            builder.RegisterComponentInHierarchy<CustomizationMenuPresenter>();
            builder.RegisterComponentInHierarchy<ChipSelectMenuPresenter>();
            builder.RegisterComponentInHierarchy<ChipColorSelectMenuPresenter>();
            builder.RegisterComponentInHierarchy<BoardSelectMenuPresenter>();
            builder.RegisterComponentInHierarchy<FloorSelectMenuPresenter>();
            builder.RegisterComponentInHierarchy<SettingsMenuPresenter>();
        }
    }
}
