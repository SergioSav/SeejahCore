using Assets.Scripts.Core.Framework;
using Assets.Scripts.Core.Models;
using UniRx;
using VContainer.Unity;

namespace Assets.Scripts.Core.Controllers
{
    public interface IGame : IStartable
    {
    }

    public class Game : DisposableContainer, IGame
    {
        private LifetimeScope _coreScope;
        private readonly GameModel _gameModel;

        public Game(LifetimeScope coreScope, GameModel gameModel)
        {
            _coreScope = coreScope;
            _gameModel = gameModel;
        }

        public void Start()
        {
            AddForDispose(_gameModel.CurrentGameState.Subscribe(OnGameStateChange));
            _gameModel.ChangeGameStateTo(GameState.Boot);
        }

        private void OnGameStateChange(GameState gameState)
        {
            UnityEngine.Debug.Log($"GAME {gameState}");
            SwitchStateTo(gameState);
        }

        private void SwitchStateTo(GameState stateName)
        {
            switch (stateName)
            {
                case GameState.Boot:
                    _gameModel.ChangeGameStateTo(GameState.Loading);
                    break;
                case GameState.Loading:
                    _gameModel.ChangeGameStateTo(GameState.MainMenu);
                    break;
                case GameState.MainMenu:
                    // NOP
                    break;
                case GameState.PrepareMatch:
                    _gameModel.ChangeGameStateTo(GameState.Match);
                    break;
                case GameState.Match:
                    // NOP
                    break;
                case GameState.Reward:
                    // NOP
                    break;
                case GameState.Settings:
                    break;
                case GameState.Customization:
                    break;
            }

        }

    }
}
