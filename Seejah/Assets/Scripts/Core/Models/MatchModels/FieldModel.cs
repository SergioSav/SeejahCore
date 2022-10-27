using Assets.Scripts.Core.Framework;
using Assets.Scripts.Core.Rules;
using Assets.Scripts.Core.Utils;
using System.Collections.Generic;
using System.Linq;
using UniRx;

namespace Assets.Scripts.Core.Models
{
    public class FieldModel : DisposableContainer
    {
        public IReadOnlyReactiveProperty<CellModel> AddChip => _addChip;
        private ReactiveProperty<CellModel> _addChip;
        public IReadOnlyReactiveProperty<CellModel> MoveChip => _moveChip;
        private ReactiveProperty<CellModel> _moveChip;
        public IReadOnlyReactiveProperty<AttackThreesome> AttackChip => _attackChip;
        private ReactiveProperty<AttackThreesome> _attackChip;
        public IReadOnlyReactiveProperty<List<CellModel>> UpdateCells => _updateCells;
        private ReactiveProperty<List<CellModel>> _updateCells;

        private Dictionary<RowColPair, CellModel> _cells;
        private RowColPair _cachedRowColPairForCompare;
        private ITimeService _timeService;
        private readonly GameRules _gameRules;

        public List<CellModel> Cells => _cells.Values.ToList();

        public CellModel SelectedCell { get; private set; }
        public int ChipCountForOnePlayer => _gameRules.ChipStartCount;

        public FieldModel(GameRules gameRules, ITimeService timeService)
        {
            _gameRules = gameRules;
            _timeService = timeService;
            _cachedRowColPairForCompare = new RowColPair();
            _cells = new Dictionary<RowColPair, CellModel>();
            SelectedCell = null;

            _addChip = AddForDispose(new ReactiveProperty<CellModel>());
            _moveChip = AddForDispose(new ReactiveProperty<CellModel>());
            _attackChip = AddForDispose(new ReactiveProperty<AttackThreesome>());
            _updateCells = AddForDispose(new ReactiveProperty<List<CellModel>>());
        }

        public override void Dispose()
        {
            base.Dispose();

            SelectedCell = null;
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
                if (cell.IsCentral || cell.Chip != null)
                    return false;
                var chip = new ChipModel(team);
                cell.SetChip(chip);
                _addChip.SetValueAndForceNotify(cell);
                return true;
            }
            return false;
        }

        public bool TrySelectChip(int row, int col, TeamType team)
        {
            if (GetCellInPosition(row, col, out var cell))
            {
                if (cell.Chip == null || cell.Chip.Team != team)
                    return false;
                SelectedCell = cell;
                return true;
            }
            return false;
        }

        public bool TryMoveSelectedChip(int row, int col)
        {
            if (GetCellInPosition(row, col, out var cell))
            {
                if (cell.Chip != null)
                {
                    UnityEngine.Debug.Log("Wrong move! Already has chip");
                    return false;
                }
                if (!cell.TestDistanceToCell(SelectedCell, _gameRules.ChipMoveDistance))
                {
                    UnityEngine.Debug.LogWarning("Wrong move! Distance");
                    return false;
                }

                MakeMoveTo(cell);
                return true;
            }
            return false;
        }

        public List<AttackThreesome> DetermineAttackThreesome(int row, int col, TeamType movingTeam)
        {
            if (GetCellInPosition(row, col, out var cell))
            {
                var cellsForUpdating = new List<AttackThreesome>();

                foreach (var shifts in _gameRules.MoveVariants)
                {
                    if (GetCellInPosition(cell.RowColPair.Row + shifts.Item1, cell.RowColPair.Col + shifts.Item2, out var neighbourCell))
                    {
                        if (neighbourCell.Chip != null && neighbourCell.Chip.Team != movingTeam)
                        {
                            if (GetCellInPosition(cell.RowColPair.Row + shifts.Item1 * 2, cell.RowColPair.Col + shifts.Item2 * 2, out var otherPlayerCell))
                            {
                                if (otherPlayerCell.Chip != null && otherPlayerCell.Chip.Team == movingTeam)
                                {
                                    var threesome = new AttackThreesome { FirstAttacker = cell, SecondAttacker = otherPlayerCell, Victim = neighbourCell };
                                    cellsForUpdating.Add(threesome);
                                }
                            }
                        }
                    }
                }
                return cellsForUpdating;
            }
            return null;
        }

        public void PrintFieldForDebug()
        {
            var counter = 5;
            var stroke = "";
            foreach (var kvp in _cells)
            {
                var content = kvp.Value.Chip != null ? kvp.Value.Chip.Team.ToString() : "---";
                stroke += $"{kvp.Key}({content})";
                counter--;
                if (counter == 0)
                {
                    UnityEngine.Debug.Log($" {stroke}");
                    counter = 5;
                    stroke = "";
                }
            }
        }

        private void MakeMoveTo(CellModel targetCell)
        {
            targetCell.SetChip(SelectedCell.Chip);
            _moveChip.SetValueAndForceNotify(targetCell);
            SelectedCell.ClearChip();
            SelectedCell = null;
        }

        public void HandleChipAttack(List<AttackThreesome> attackingCells)
        {
            for (var i = 0; i < attackingCells.Count; i++)
            {
                AttackThreesome threesome = attackingCells[i];
                threesome.Victim.ClearChip();
                _timeService.Wait(i).Then(() => _attackChip.SetValueAndForceNotify(threesome));
            }
        }
    }
}
