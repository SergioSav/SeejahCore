namespace Assets.Scripts.Core.Rules
{
    public class GameRules
    {
        private int _fieldRowCount;
        private int _fieldColCount;
        private int _chipPlacementCount;
        private int _chipStartCount;
        private int _chipMoveDistance;
        private int _minimalChipCountInGame;

        public GameRules()
        {
            _fieldRowCount = 5;
            _fieldColCount = 5;
            _chipPlacementCount = 2;
            _chipStartCount = 12;
            _chipMoveDistance = 1;
            _minimalChipCountInGame = 2;
        }

        public int RowCount => _fieldRowCount;
        public int ColCount => _fieldColCount;
        public int ChipPlacementCount => _chipPlacementCount;
        public int ChipStartCount => _chipStartCount;
        public int ChipMoveDistance => _chipMoveDistance;

        public int MinimalChipCountInGame => _minimalChipCountInGame;
    }
}
