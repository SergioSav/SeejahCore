using Assets.Scripts.Core.Framework;
using Assets.Scripts.Core.Models.AILogic;
using Assets.Scripts.Core.Rules;
using Assets.Scripts.Core.Utils;

namespace Assets.Scripts.Core.Models
{
    public class AIAdaptiveBrainModel : DisposableContainer, IAIBrain
    {
        private readonly ILogic _placementLogic;
        private readonly ILogic _battleLogic;
        private readonly GameRules _gameRules;
        private ILogic _logic;

        public AIAdaptiveBrainModel(GameRules gameRules, FieldModel fieldModel, RandomProvider random, TeamType teamType)
        {
            _gameRules = gameRules;

            _placementLogic = new AIPlacementLogic(fieldModel, random);
            _battleLogic = new AIAdaptiveBattleLogic(gameRules, fieldModel, random, teamType);
            Reset();
        }

        public bool IsHuman => false;

        public void Reset()
        {
            SwitchToPlacement();
        }

        public void ResetLogic()
        {
            _logic.Reset();
        }

        public void SwitchToBattle()
        {
            _logic = _battleLogic; 
        }

        public void SwitchToPlacement()
        {
            _logic = _placementLogic;
        }

        public bool TryGetCellForSelect(out CellModel resultCell)
        {
            resultCell = _logic.CellForSelect();
            return resultCell != default;
        }

        public bool TryGetCellForMove(out CellModel resultCell)
        {
            resultCell = _logic.CellForMove();
            return resultCell != default;
        }

        public void TuneDifficulty(int winStreak, int loseStreak)
        {
            var difficulty = AIBrainDifficulty.Medium;
            if (loseStreak > _gameRules.LoseStreakLimit)
                difficulty = AIBrainDifficulty.Easy;
            else if (winStreak >= _gameRules.UltimateWinStreakLimit)
                difficulty = AIBrainDifficulty.Ultimate;
            else if (winStreak >= _gameRules.WinStreakLimit)
                difficulty = AIBrainDifficulty.Hard;
            _battleLogic.SetupDifficulty(difficulty);
        }
    }
}
