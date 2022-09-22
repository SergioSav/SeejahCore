using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.Presenters;
using Assets.Scripts.Core.Rules;
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
                .WithParameter(DateTime.Now.Millisecond);
            builder.RegisterEntryPoint<TimeService>(Lifetime.Singleton);

            builder.Register<GameRules>(Lifetime.Singleton);    // TEMP
            builder.Register<GameModel>(Lifetime.Singleton);    // TEMP

            builder.Register<ISceneLoader, SceneLoader>(Lifetime.Singleton);
            builder.RegisterComponent(gamePresenter);

            builder.RegisterEntryPoint<Game>(Lifetime.Singleton);
        }
    }
}