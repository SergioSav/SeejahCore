using Assets.Scripts.Core.Framework;
using Assets.Scripts.Core.Rules;
using System;
using UniRx;

namespace Assets.Scripts.Core.Models
{
    public class MatchModel : DisposableContainer
    {
        private ReactiveProperty<MatchStateType> _currentState;
        private ReactiveProperty<PlayerModel> _currentPlayer;
        private int _placementChipCount;
        private int _totalChipCount;

        private readonly GameRules _rules;
        private readonly GameModel _gameModel;

        public IReadOnlyReactiveProperty<MatchStateType> CurrentState => _currentState;
        public IReadOnlyReactiveProperty<PlayerModel> CurrentPlayer => _currentPlayer;

        public MatchModel(GameRules rules, GameModel gameModel)
        {
            _rules = rules;
            _gameModel = gameModel;

            _currentState = AddForDispose(new ReactiveProperty<MatchStateType>(MatchStateType.None));
            _currentPlayer = AddForDispose(new ReactiveProperty<PlayerModel>(_gameModel.ChooseFirstPlayer()));
        }

        public void SwitchStateTo(MatchStateType state)
        {
            _currentState.Value = state;
        }

        public void HandleChipPlace()
        {
            _totalChipCount++;
            _placementChipCount++;
            _currentPlayer.Value.AddChipInGame();
            if (_placementChipCount == _rules.ChipPlacementCount)
            {
                _currentPlayer.Value = _gameModel.NextPlayer();
                _placementChipCount = 0;
            }
            if (_totalChipCount >= _rules.ChipStartCount * _gameModel.PlayerCount)
            {
                SwitchStateTo(MatchStateType.ReadyForPlay);
            }
        }

        public void HandleEndTurn()
        {
            //SwitchStateTo(MatchStateType.WaitNextPlayer);
            _currentPlayer.Value = _gameModel.NextPlayer();
            if (TestEndMatchCondition())
                SwitchStateTo(MatchStateType.EndMatch);
        }

        private bool TestEndMatchCondition()
        {
            var result = false;
            foreach (var player in _gameModel.Players)
            {
                result |= !player.HasEnoughChipInGame();
            }
            
            return result;
        }
    }
}
