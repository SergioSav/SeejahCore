using Assets.Scripts.Core.Controllers;
using Assets.Scripts.Core.Framework;
using UniRx;

namespace Assets.Scripts.Core.Models
{
    public class GameModel : DisposableContainer
    {
        private ReactiveProperty<GameState> _currentState;

        public GameModel()
        {
            _currentState = AddForDispose(new ReactiveProperty<GameState>());
        }

        public IReadOnlyReactiveProperty<GameState> CurrentGameState => _currentState;

        public void ChangeGameStateTo(GameState gameState)
        {
            UnityEngine.Debug.Log($"GameModel {gameState}");
            _currentState.Value = gameState;
        }

        public void StartMatch()
        {
            ChangeGameStateTo(GameState.PrepareMatch);
        }

        public void StartCusomization()
        {
            ChangeGameStateTo(GameState.Customization);
        }

        public void EndCustomization()
        {
            ChangeGameStateTo(GameState.MainMenu);
        }
    }
}
