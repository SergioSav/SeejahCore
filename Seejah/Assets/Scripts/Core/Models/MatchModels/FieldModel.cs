using Assets.Scripts.Core.Framework;
using Assets.Scripts.Core.Rules;
using System.Collections.Generic;
using System.Linq;
using UniRx;

namespace Assets.Scripts.Core.Models
{
    public class FieldModel : DisposableContainer
    {
        public IReadOnlyReactiveProperty<CellModel> AddChip => _addChip;
        private ReactiveProperty<CellModel> _addChip;

        public IReadOnlyReactiveProperty<List<CellModel>> UpdateCells => _updateCells;
        private ReactiveProperty<List<CellModel>> _updateCells;

        private Dictionary<RowColPair, CellModel> _cells;
        private RowColPair _cachedRowColPairForCompare;
        private readonly GameRules _gameRules;

        public List<CellModel> Cells => _cells.Values.ToList();

        public FieldModel(GameRules gameRules)
        {
            _gameRules = gameRules;
            _cachedRowColPairForCompare = new RowColPair();
            _cells = new Dictionary<RowColPair, CellModel>();

            _addChip = AddForDispose(new ReactiveProperty<CellModel>());
            _updateCells = AddForDispose(new ReactiveProperty<List<CellModel>>());
        }

        public void CreateField(int row, int col)
        {
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    var cell = new CellModel(i, j);
                    if (i == _gameRules.RowCount / 2 && j == _gameRules.ColCount / 2)
                        cell.SetCentral();
                    _cells[cell.RowColPair] = cell;
                }
            }
            _updateCells.SetValueAndForceNotify(_cells.Values.ToList());
        }

        public bool GetCellInPosition(int row, int col, out CellModel cell)
        {
            _cachedRowColPairForCompare.Row = row;
            _cachedRowColPairForCompare.Col = col;
            return _cells.TryGetValue(_cachedRowColPairForCompare, out cell);
        }

        public bool TryGenerateChip(int row, int col, TeamType team)
        {
            if (GetCellInPosition(row, col, out var cell))
            {
                if (cell.IsCentral)
                    return false;
                var chip = new ChipModel(team);
                cell.SetChip(chip);
                _addChip.SetValueAndForceNotify(cell);
                return true;
            }
            return false;
        }
    }
}
