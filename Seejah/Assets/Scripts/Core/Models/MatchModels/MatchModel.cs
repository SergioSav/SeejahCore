using Assets.Scripts.Core.Framework;
using Assets.Scripts.Core.Rules;
using System.Linq;
using UniRx;

namespace Assets.Scripts.Core.Models
{
    public class MatchModel : DisposableContainer
    {
        private ReactiveProperty<MatchStateType> _currentState;
        private ReactiveProperty<PlayerModel> _currentPlayer;
        private int _placementChipCount;

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
            UnityEngine.Debug.Log($"Match state: {state}");
            _currentState.Value = state;

            switch (state)
            {
                case MatchStateType.None:
                    break;
                case MatchStateType.WaitPlaceChip:
                    _currentPlayer.Value.MakeTurn();
                    break;
                case MatchStateType.WaitNextPlayer:
                    break;
                case MatchStateType.ReadyForBattle:
                    break;
                case MatchStateType.WaitChipMove:
                    _currentPlayer.Value.MakeTurn();
                    break;
                case MatchStateType.WaitNextTurn:
                    break;
                case MatchStateType.EndMatch:
                    break;
            }
        }

        public void HandleChipPlace()
        {
            _placementChipCount++;
            _currentPlayer.Value.AddChipInGame();
            if (_placementChipCount == _rules.ChipPlacementCount)
            {
                _currentPlayer.Value = _gameModel.NextPlayer();
                SwitchStateTo(MatchStateType.WaitNextPlayer);
                _placementChipCount = 0;
                SwitchStateTo(MatchStateType.WaitPlaceChip);
            }
            else
            {
                _currentPlayer.Value.MakeTurn();
            }

            if (_currentState.Value == MatchStateType.WaitPlaceChip && _gameModel.Players.All(p => p.ReadyForBattle))
            {
                SwitchStateTo(MatchStateType.ReadyForBattle);
            }
        }

        public void HandleRemoveChip(TeamType team)
        {
            foreach (var player in _gameModel.Players)
            {
                if (player.TeamType == team)
                    player.RemoveInGameChip();
            }
        }

        public void HandleEndTurn()
        {
            SwitchStateTo(MatchStateType.WaitNextPlayer);
            _currentPlayer.Value = _gameModel.NextPlayer();
            SwitchStateTo(MatchStateType.WaitChipMove);
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
