using System;

namespace Assets.Scripts.Core.Data
{
    [Serializable]
    public class GameRulesData
    {
        public int FieldRowCount;
        public int FieldColCount;
        public int ChipPlacementCount;
        public int ChipStartCount;
        public int ChipMoveDistance;
        public int MinimalChipCountInGame;
    }
}
