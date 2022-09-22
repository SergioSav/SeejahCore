using Assets.Scripts.Core.Framework;
using Assets.Scripts.Core.Rules;
using Assets.Scripts.Core.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Core.Models.AILogic
{
    public class AIBattleLogic : DisposableContainer, ILogic
    {
        private readonly GameRules _gameRules;
        private readonly FieldModel _fieldModel;
        private readonly RandomProvider _random;
        private TeamType _currentTeam;
        private CellModel _selectedCell;
        private CellModel _cellForMove;

        public AIBattleLogic(GameRules gameRules, FieldModel fieldModel, RandomProvider random, TeamType teamType)
        {
            _gameRules = gameRules;
            _fieldModel = fieldModel;
            _random = random;
            _currentTeam = teamType;
        }

        public void Reset()
        {
            _selectedCell = default;
            _cellForMove = default;
        }

        public CellModel CellForSelect()
        {
            DefineCells();
            return _selectedCell;
        }

        public CellModel CellForMove()
        {
            return _cellForMove;
        }

        private void DefineCells()
        {
            //    // choose for move
            //    // 1. find all self chips
            //    // 2. get all with move available
            //    // 3. choose more efficient


            // stupid logic
            var allMyChipCells = _fieldModel.Cells.Where(c => c.Chip != null && c.Chip.Team == _currentTeam);

            var moveVariants = new List<(CellModel, CellModel)>();
            foreach (var cell in allMyChipCells)
            {
                foreach (var shifts in _gameRules.MoveVariants)
                {
                    if (_fieldModel.GetCellInPosition(cell.RowColPair.Row + shifts.Item1, cell.RowColPair.Col + shifts.Item2, out var neighbourCell))
                    {
                        if (neighbourCell.Chip == null)
                            moveVariants.Add((neighbourCell, cell));
                    }
                }
            }
            if (_random.GetRandom(moveVariants, out var resultCell))
            {
                _selectedCell = resultCell.Item2;
                _cellForMove = resultCell.Item1;
            }
        }
    }
}
