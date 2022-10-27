using Assets.Scripts.Core.Controllers;
using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.Presenters;
using Assets.Scripts.Core.Utils;
using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Assets.Scripts.Core
{
    public class BootScript : MonoBehaviour
    {
        [SerializeField]
        private GamePresenter gamePresenter;

        private void Start()
        {
            var coreScope = gameObject.AddComponent<CoreLifetimeScope>();
            coreScope.CreateChild(InstallDependencies);

            DontDestroyOnLoad(this);
        }

        private void InstallDependencies(IContainerBuilder builder)
        {
            builder.Register<RandomProvider>(Lifetime.Singleton)
                .WithParameter((int)DateTimeOffset.Now.ToUnixTimeSeconds());
            builder.RegisterEntryPoint<TimeService>(Lifetime.Singleton);

            builder.Register<ISceneLoader, SceneLoader>(Lifetime.Singleton);
            builder.RegisterComponent(gamePresenter);

            builder.Register<GameModel>(Lifetime.Singleton);
            builder.RegisterEntryPoint<Game>(Lifetime.Singleton);
        }
    }
}