using System.Collections.Generic;

namespace Assets.Scripts.Core.Models.AILogic
{
    public class AIMoveLogicData
    {
        public CellModel TargetCell;
        public List<CellModel> PossibleMoveFromCells;
        public int PossibleAttackCount;
        public int PossibleThreatCount;

        public override string ToString()
        {
            var possibleMoveFrom = string.Join("|", PossibleMoveFromCells);
            return $"{TargetCell} <-- {possibleMoveFrom}. A: {PossibleAttackCount}, T:{PossibleThreatCount}";
        }
    }
}
