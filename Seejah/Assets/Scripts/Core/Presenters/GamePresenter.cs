using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.SceneInstallers;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;

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
            Debug.Log($"GamePresenter {gameState}");
            switch (gameState)
            {
                case GameState.None:
                    break;
                case GameState.Boot:
                    break;
                case GameState.Loading:
                    _parentScope.CreateChild(new LoadingInstaller());
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

    internal class LoadingInstaller : IInstaller
    {
        public void Install(IContainerBuilder builder)
        {
            
        }
    }
}
