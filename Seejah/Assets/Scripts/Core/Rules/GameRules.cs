using Assets.Scripts.Core.Data;
using System.Collections.Generic;

namespace Assets.Scripts.Core.Rules
{
    public class GameRules
    {
        private readonly GameRulesData _data;
        private readonly List<(int, int)> _moveVariants;

        public GameRules(GameRulesData data)
        {
            _data = data;
            _moveVariants = new List<(int, int)>
                    {
                        (-1, -1),
                        (-1, 0),
                        (-1,1),
                        (0,-1),
                        (0,1),
                        (1,-1),
                        (1,0),
                        (1,1)
                    };
        }

        public int RowCount => _data.FieldRowCount;
        public int ColCount => _data.FieldColCount;
        public int ChipPlacementCount => _data.ChipPlacementCount;
        public int ChipStartCount => _data.ChipStartCount;
        public int ChipMoveDistance => _data.ChipMoveDistance;

        public int MinimalChipCountInGame => _data.MinimalChipCountInGame;

        public List<(int, int)> MoveVariants => _moveVariants;
    }
}
