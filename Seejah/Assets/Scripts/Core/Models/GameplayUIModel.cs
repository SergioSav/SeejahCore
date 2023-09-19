using Assets.Scripts.Core.Framework;
using UniRx;

namespace Assets.Scripts.Core.Models
{
    public class GameplayUIModel : DisposableContainer
    {
        private readonly ReactiveProperty<GameplayUIState> _currentState;

        public GameplayUIModel()
        {
            _currentState = AddForDispose(new ReactiveProperty<GameplayUIState>());
        }

        public IReadOnlyReactiveProperty<GameplayUIState> CurrentState => _currentState;

        public void ChangeStateTo(GameplayUIState state)
        {
            UnityEngine.Debug.Log($"GameplayUIModel {state}");
            _currentState.Value = state;
        }

        public void ReturnNormalState()
        {
            ChangeStateTo(GameplayUIState.Normal);
        }

        public void ShowTutorialWindow()
        {
            ChangeStateTo(GameplayUIState.TutorialWindow);
        }
    }
}