using Assets.Scripts.Core.Framework;
using Assets.Scripts.Core.Models.MatchModels;
using Assets.Scripts.Core.Utils;
using System.Collections.Generic;
using System.Linq;
using UniRx;

namespace Assets.Scripts.Core.Models
{
    public class MatchModel : DisposableContainer
    {
        private readonly RandomProvider _random;
        private readonly UserModel _userModel;
        private readonly MatchOptions _options;

        private List<IPlayerModel> _players;
        private IPlayerModel _activePlayer;
        private ReactiveProperty<MatchStateType> _currentState;
        private Subject<Unit> _waitNextTurn;

        public Subject<Unit> WaitNextTurn => _waitNextTurn;
        public IReadOnlyReactiveProperty<MatchStateType> CurrentState => _currentState;

        public IPlayerModel ActivePlayer => _activePlayer;
        public MatchOptions Options => _options;
        public bool IsUserTurn => _activePlayer.TeamType == _userModel.TeamType;

        public MatchModel(RandomProvider random, UserModel userModel)
        {
            _random = random;
            _userModel = userModel;
            _options = new MatchOptions
            {
                ChipId = userModel.SelectedChipId
            };

            _currentState = AddForDispose(new ReactiveProperty<MatchStateType>(MatchStateType.None));
            _waitNextTurn = AddForDispose(new Subject<Unit>());
        }

        public void AddPlayers(List<IPlayerModel> players)
        {
            _players = players;
        }

        public IPlayerModel ChooseFirstPlayer()
        {
            _random.GetRandom(_players, out var player);
            _activePlayer = player;
            return player;
        }

        public IPlayerModel ChooseNextPlayer()
        {
            var index = _players.IndexOf(_activePlayer);
            var nextIndex = (index + 1) % _players.Count;
            _activePlayer = _players[nextIndex];
            return _players[nextIndex];
        }

        public void HandleRemoveChip(TeamType team)
        {
            foreach (var player in _players)
            {
                if (player.TeamType == team)
                    player.RemoveInGameChip();
            }
        }

        private bool TestEndMatchCondition()
        {
            var result = false;
            foreach (var player in _players)
                result |= !player.HasEnoughChipInGame();

            return result;
        }

        public IPlayerModel GetWinner()
        {
            foreach (var player in _players)
            {
                if (player.HasEnoughChipInGame())
                {
                    return player;
                }
            }
            return default;
        }

        private void SwitchStateTo(MatchStateType state)
        {
            UnityEngine.Debug.Log($"Match state: {state}");
            _currentState.Value = state;
        }

        public void SetLoading()
        {
            SwitchStateTo(MatchStateType.Loading);
        }

        public void SetReady()
        {
            SwitchStateTo(MatchStateType.Ready);
        }

        public void StartPlacement()
        {
            SwitchStateTo(MatchStateType.PhasePlacement);
            _waitNextTurn.OnNext(Unit.Default);
        }

        public void StartBattle()
        {
            SwitchStateTo(MatchStateType.PhaseBattle);
            _waitNextTurn.OnNext(Unit.Default);
        }

        public void HandleEndTurn()
        {
            if (_currentState.Value == MatchStateType.PhasePlacement && _players.All(p => p.ReadyForBattle))
                SwitchStateTo(MatchStateType.PlacementDone);
            if (_currentState.Value == MatchStateType.PhaseBattle && TestEndMatchCondition())
                SwitchStateTo(MatchStateType.BattleEnd);
            _waitNextTurn.OnNext(Unit.Default);
        }

        public override void Dispose()
        {
            // reset data
        }
    }
}
