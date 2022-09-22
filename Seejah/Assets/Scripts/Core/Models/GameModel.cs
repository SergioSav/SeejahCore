using Assets.Scripts.Core.Framework;
using UniRx;

namespace Assets.Scripts.Core.Models
{
    public class GameModel : DisposableContainer
    {
        private ReactiveProperty<GameState> _currentState;
        private ReactiveProperty<bool> _needStartMatch;

        public GameModel()
        {
            _currentState = AddForDispose(new ReactiveProperty<GameState>());
            _needStartMatch = AddForDispose(new ReactiveProperty<bool>());
        }

        public IReadOnlyReactiveProperty<GameState> CurrentGameState => _currentState;
        public IReadOnlyReactiveProperty<bool> NeedStartMatch => _needStartMatch;

        public void ChangeGameStateTo(GameState gameState)
        {
            UnityEngine.Debug.Log($"GameModel {gameState}");
            _currentState.Value = gameState;
        }

        public void StartMatch()
        {
            _needStartMatch.Value = true;
        }

        public void EndMatch()
        {
            _needStartMatch.Value = false;
        }
    }
}
