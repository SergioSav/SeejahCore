using Assets.Scripts.Core.Framework;
using Assets.Scripts.Core.Rules;
using Assets.Scripts.Core.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Core.Models.AILogic
{
    public class AIUltimateBattleLogic : DisposableContainer, ILogic
    {
        private readonly GameRules _gameRules;
        private readonly FieldModel _fieldModel;
        private readonly RandomProvider _random;
        private TeamType _currentTeam;
        private CellModel _selectedCell;
        private CellModel _cellForMove;

        public AIUltimateBattleLogic(GameRules gameRules, FieldModel fieldModel, RandomProvider random, TeamType teamType)
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
            var moveVariants = GetMoveVariants();

            var cellsWithPossibleMoveFromCells = moveVariants
                .Where(v => v.PossibleMoveFromCells.Count > 0)
                .ToList();
            var decisionWithPossibleAttack = cellsWithPossibleMoveFromCells
                .Where(v => v.PossibleAttackCount > 0)
                .OrderByDescending(v => v.PossibleAttackCount)
                .FirstOrDefault();
            if (decisionWithPossibleAttack != default)
            {
                if (decisionWithPossibleAttack.PossibleMoveFromCells.Count > 1)
                {
                    if (_random.GetRandom(decisionWithPossibleAttack.PossibleMoveFromCells, out var selectedCell))
                        _selectedCell = selectedCell;
                }
                else
                {
                    _selectedCell = decisionWithPossibleAttack.PossibleMoveFromCells.FirstOrDefault();
                }
                _cellForMove = decisionWithPossibleAttack.TargetCell;
            }
            else
            {
                var variantsWithoutThreat = cellsWithPossibleMoveFromCells
                    .Where(v => v.PossibleThreatCount == 0)
                    .ToList();
                if (variantsWithoutThreat.Count > 0)
                {
                    if (_random.GetRandom(variantsWithoutThreat, out var variant))
                    {
                        if (variant.PossibleMoveFromCells.Count > 1)
                        {
                            if (_random.GetRandom(variant.PossibleMoveFromCells, out var selectedCell))
                                _selectedCell = selectedCell;
                        }
                        else
                        {
                            _selectedCell = variant.PossibleMoveFromCells.FirstOrDefault();
                        }
                        _cellForMove = variant.TargetCell;
                    }
                }
                else
                {
                    if (_random.GetRandom(cellsWithPossibleMoveFromCells, out var variant))
                    {
                        if (variant.PossibleMoveFromCells.Count > 1)
                        {
                            if (_random.GetRandom(variant.PossibleMoveFromCells, out var selectedCell))
                                _selectedCell = selectedCell;
                        }
                        else
                        {
                            _selectedCell = variant.PossibleMoveFromCells.FirstOrDefault();
                        }
                        _cellForMove = variant.TargetCell;
                    }
                }
            }
            
        }

        private List<AIMoveLogicData> GetMoveVariants()
        {
            var moveVariants = new List<AIMoveLogicData>();

            var allEmptyCells = _fieldModel.Cells
                .Where(c => c.Chip == null);

            foreach (var cell in allEmptyCells)
            {
                var data = new AIMoveLogicData { TargetCell = cell, PossibleMoveFromCells = new List<CellModel>() };
                foreach (var shifts in _gameRules.MoveVariants)
                {
                    if (GetCellWithShift(cell, shifts, out var neighbourCell))
                    {
                        if (neighbourCell.Chip != null && neighbourCell.Chip.Team == _currentTeam)
                        {
                            data.PossibleMoveFromCells.Add(neighbourCell);
                        }
                        CalculatePossibleAttackThreatCounts(cell, data);
                    }
                }
                moveVariants.Add(data);
            }

            return moveVariants;
        }

        private bool GetCellWithShift(CellModel cell, (int, int) shift, out CellModel resultCell, int shiftMultiplier = 1)
        {
            var shiftedRow = cell.RowColPair.Row + shift.Item1 * shiftMultiplier;
            var shiftedCol = cell.RowColPair.Col + shift.Item2 * shiftMultiplier;
            return _fieldModel.GetCellInPosition(shiftedRow, shiftedCol, out resultCell);
        }

        private void CalculatePossibleAttackThreatCounts(CellModel cell, AIMoveLogicData data)
        {
            var attackCount = 0;
            var threatCount = 0;
            foreach (var shifts in _gameRules.MoveVariants)
            {
                if (GetCellWithShift(cell, shifts, out var neighbourCell))
                {
                    if (neighbourCell.Chip != null && neighbourCell.Chip.Team != _currentTeam)
                    {
                        if (GetCellWithShift(cell, shifts, out var otherPlayerCell, 2))
                        {
                            if (otherPlayerCell.Chip != null && otherPlayerCell.Chip.Team == _currentTeam)
                            {
                                attackCount++;
                            }
                            else
                            {
                                threatCount++; // TODO: not ultimate variant
                            }
                        }
                    }
                }
            }
            data.PossibleAttackCount = attackCount;
            data.PossibleThreatCount = threatCount;
        }
    }

    public class AIMoveLogicData
    {
        public CellModel TargetCell;
        public List<CellModel> PossibleMoveFromCells;
        public int PossibleAttackCount;
        public int PossibleThreatCount;
    }
}
