using Assets.Scripts.Core.Commands;
using Assets.Scripts.Core.HUD;
using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.Presenters;
using Assets.Scripts.Core.Views;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Assets.Scripts.Core.SceneInstallers
{
    public class GameplayExtrasInstaller : IInstaller
    {
        public void Install(IContainerBuilder builder)
        {
            builder.Register<FieldModel>(Lifetime.Singleton);
            builder.Register<MatchModel>(Lifetime.Singleton);

            builder.Register<IPlayerModel, PlayerModel>(Lifetime.Transient);
            builder.RegisterFactory<TeamType, IBrain, IPlayerModel>(container =>
            {
                return (team, brain) => {
                    var player = container.Resolve<IPlayerModel>();
                    player.Setup(team, brain);
                    return player;
                };
            },
            Lifetime.Singleton);

            builder.RegisterFactory<CellView, Transform, CellView>(container =>
            {
                return (prefab, parentTransform) => container.Instantiate(prefab, parentTransform);
            },
            Lifetime.Singleton);
            builder.RegisterFactory<ChipView, Transform, ChipView>(container =>
            {
                return (prefab, parentTransform) => container.Instantiate(prefab, parentTransform);
            },
            Lifetime.Singleton);

            builder.Register<ISelectCellCommand, SelectCellCommand>(Lifetime.Singleton);

            builder.RegisterComponentInHierarchy<FieldPresenter>();
            builder.RegisterComponentInHierarchy<GameplayUIPresenter>();

            builder.RegisterEntryPoint<Match>(Lifetime.Singleton);
        }
    }
}
