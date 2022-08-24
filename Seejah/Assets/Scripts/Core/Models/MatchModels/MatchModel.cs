using Assets.Scripts.Core.Framework;
using Assets.Scripts.Core.Rules;
using UniRx;

namespace Assets.Scripts.Core.Models
{
    public class MatchModel : DisposableContainer
    {
        private ReactiveProperty<MatchStateType> _currentState;
        private PlayerModel _currentPlayer;
        private int _placementChipCount;
        private int _totalChipCount;

        private readonly GameRules _rules;
        private readonly GameModel _gameModel;

        public IReadOnlyReactiveProperty<MatchStateType> CurrentState => _currentState;
        public PlayerModel CurrentPlayer => _currentPlayer;

        public MatchModel(GameRules rules, GameModel gameModel)
        {
            _rules = rules;
            _gameModel = gameModel;

            _currentState = AddForDispose(new ReactiveProperty<MatchStateType>(MatchStateType.None));
            _currentPlayer = _gameModel.ChooseFirstPlayer();
        }

        public void SwitchStateTo(MatchStateType state)
        {
            _currentState.Value = state;
        }

        public void HandleChipPlace()
        {
            _totalChipCount++;
            _placementChipCount++;
            if (_placementChipCount == _rules.ChipPlacementCount)
            {
                _currentPlayer = _gameModel.NextPlayer();
                _placementChipCount = 0;
            }
            if (_totalChipCount >= _rules.ChipStartCount * _gameModel.PlayerCount)
            {
                SwitchStateTo(MatchStateType.ReadyForPlay);
            }
        }
    }
}
