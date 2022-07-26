﻿using Assets.Scripts.Core.Framework;
using Assets.Scripts.Core.Models.AILogic;
using Assets.Scripts.Core.Rules;
using Assets.Scripts.Core.Utils;

namespace Assets.Scripts.Core.Models
{
    public class AIBrainModel : DisposableContainer, IAIBrain
    {
        private readonly ILogic _placementLogic;
        private readonly ILogic _battleLogic;

        private ILogic _logic;

        public AIBrainModel(GameRules gameRules, FieldModel fieldModel, RandomProvider random, TeamType teamType)
        {
            _placementLogic = new AIPlacementLogic(fieldModel, random);
            _battleLogic = new AIBattleLogic(gameRules, fieldModel, random, teamType);
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
    }
}
