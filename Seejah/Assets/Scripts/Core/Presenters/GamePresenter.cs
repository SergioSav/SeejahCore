﻿using Assets.Scripts.Core.HUD.MainMenu;
using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.Views;
using System;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using static UnityEditor.PlayerSettings;

namespace Assets.Scripts.Core.Presenters
{
    public class GamePresenter : MonoBehPresenter
    {
        private LifetimeScope _parentScope;
        private GameModel _gameModel;
        private ISceneLoader _sceneLoader;

        [Inject]
        public void Construct(GameModel gameModel, ISceneLoader sceneLoader, LifetimeScope parentScope)
        {
            _parentScope = parentScope;
            _gameModel = gameModel;
            _sceneLoader = sceneLoader;

            AddForDispose(_gameModel.CurrentGameState.Subscribe(OnGameStateChange));
        }

        private void OnGameStateChange(GameState gameState)
        {
            UnityEngine.Debug.Log($"GamePresenter {gameState}");
            switch (gameState)
            {
                case GameState.None:
                    break;
                case GameState.Boot:
                    break;
                case GameState.Loading:
                    break;
                case GameState.MainMenu:
                    SwitchScene("Menu", new MainMenuExtrasInstaller());
                    break;
                case GameState.PrepareMatch:
                    SwitchScene("Gameplay", new GameplayExtrasInstaller());
                    break;
                case GameState.Match:
                    break;
                case GameState.Reward:
                    break;
                case GameState.Settings:
                    break;
                case GameState.Customization:
                    break;
                default:
                    break;
            }
        }

        private void SwitchScene(string sceneName, IInstaller extraInstaller)
        {
            StartCoroutine(_sceneLoader.LoadScene(sceneName, extraInstaller));
        }
    }

    public class MainMenuExtrasInstaller : IInstaller
    {
        public void Install(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<MainMenuPresenter>();
        }
    }

    public class GameplayExtrasInstaller : IInstaller
    {
        public void Install(IContainerBuilder builder)
        {
            builder.Register<FieldModel>(Lifetime.Singleton);
            builder.Register<MatchModel>(Lifetime.Singleton);

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

            builder.RegisterComponentInHierarchy<FieldPresenter>();
        }
    }
}