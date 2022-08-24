using Assets.Scripts.Core.Framework;
using Assets.Scripts.Core.Rules;
using Assets.Scripts.Core.Utils;
using System;
using UniRx;

namespace Assets.Scripts.Core.Models
{
    public class MatchModel : DisposableContainer
    {
        private ReactiveProperty<MatchStateType> _currentState;
        private TeamType _currentTeam;
        private int _placementChipCount;

        private readonly RandomProvider _randomProvider;
        private readonly GameRules _rules;

        public IReadOnlyReactiveProperty<MatchStateType> CurrentState => _currentState;
        public TeamType CurrentTeam => _currentTeam;

        public MatchModel(RandomProvider randomProvider, GameRules rules)
        {
            _randomProvider = randomProvider;
            _rules = rules;

            _currentState = AddForDispose(new ReactiveProperty<MatchStateType>(MatchStateType.None));
            DetermineFirstTurnTeam();
        }

        public void SwitchStateTo(MatchStateType state)
        {
            _currentState.Value = state;
        }

        public void HandleChipPlace()
        {
            _placementChipCount++;
            if (_placementChipCount == _rules.ChipPlacementCount)
            {
                SetNextTeam();
                _placementChipCount = 0;
            }
        }

        private void DetermineFirstTurnTeam()
        {
            _randomProvider.GetRandom(out _currentTeam);
        }

        private void SetNextTeam()
        {
            var values = Enum.GetValues(typeof(TeamType));
            var len = values.Length;
            var nextIndex = (int)_currentTeam % (len - 1) + 1;
            _currentTeam = (TeamType)nextIndex;
        }
    }
}
