using Assets.Scripts.Core.Framework;
using Assets.Scripts.Core.Utils;
using System.Collections.Generic;
using System.Linq;
using static UnityEditor.UIElements.ToolbarMenu;

namespace Assets.Scripts.Core.Models.AILogic
{
    public class AIBattleLogic : DisposableContainer, ILogic
    {
        private readonly FieldModel _fieldModel;
        private readonly RandomProvider _random;
        private TeamType _currentTeam;

        private List<(int,int)> _variantsTemplate = new List<(int, int)>
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

        public AIBattleLogic(FieldModel fieldModel, RandomProvider random, TeamType teamType)
        {
            _fieldModel = fieldModel;
            _random = random;
            _currentTeam = teamType;
        }

        public CellModel GetCellForMove()
        {
            //    // choose for move
            //    // 1. find all self chips
            //    // 2. get all with move available
            //    // 3. choose more efficient

            // stupid logic
            var allMyChipCells = _fieldModel.Cells.Where(c => c.Chip != null && c.Chip.Team == _currentTeam).ToList();

            var moveVariants = new List<(CellModel,CellModel)>();
            foreach (var cell in allMyChipCells)
            {
                foreach (var shifts in _variantsTemplate)
                {
                    if (_fieldModel.GetCellInPosition(cell.RowColPair.Row + shifts.Item1, cell.RowColPair.Col + shifts.Item2, out var neighbourCell))
                    {
                        if (neighbourCell.Chip == null)
                            moveVariants.Add((neighbourCell,cell));
                    }
                }
            }
            if (_random.GetRandom(moveVariants, out var resultCell))
            {
                _fieldModel.TrySelectChip(resultCell.Item2.RowColPair.Row, resultCell.Item2.RowColPair.Col, _currentTeam);
                return resultCell.Item1;
            }
            
            return default;
        }
    }
}
