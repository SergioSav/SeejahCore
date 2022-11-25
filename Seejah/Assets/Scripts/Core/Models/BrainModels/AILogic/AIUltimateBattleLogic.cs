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
            UnityEngine.Debug.Log("- = ULTIMATE = -");
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
            MakeDecision();
            return _selectedCell;
        }

        public CellModel CellForMove()
        {
            return _cellForMove;
        }

        private void DefineCells(AIMoveLogicData data)
        {
            _selectedCell = data.PossibleMoveFromCells.Random(_random);
            _cellForMove = data.TargetCell;
            UnityEngine.Debug.Log($"DECISION: {data}");
        }

        private void MakeDecision()
        {
            var moveVariants = GetMoveVariants();

            var variantsWithPossibleMoveFromCells = moveVariants
                .Where(v => v.PossibleMoveFromCells.Count > 0);
            LogVariants("PossibleMove", variantsWithPossibleMoveFromCells);

            var variantsWithPossibleAttack = variantsWithPossibleMoveFromCells
                .Where(v => v.PossibleAttackCount > 0)
                .OrderByDescending(v => v.PossibleAttackCount);
            LogVariants("PossibleAttack", variantsWithPossibleAttack);
            var variant = variantsWithPossibleAttack.FirstOrDefault();

            if (variant != default)
            {
                DefineCells(variant);
            }
            else
            {
                var variantsWithoutThreat = variantsWithPossibleMoveFromCells
                    .Where(v => v.PossibleThreatCount == 0)
                    .ToList();
                if (_random.GetRandom(variantsWithoutThreat, out variant))
                {
                    DefineCells(variant);
                }
                else
                {
                    var variantsWithThreat = variantsWithPossibleMoveFromCells
                        .OrderBy(v => v.PossibleThreatCount);
                    LogVariants("PossibleThreat", variantsWithThreat);
                    variant = variantsWithThreat.FirstOrDefault();

                    if (variant != default)
                    {
                        DefineCells(variant);
                    }
                    else
                    {
                        variant = variantsWithPossibleMoveFromCells.Random(_random);
                        DefineCells(variant);
                    }
                }
            }
        }

        private void LogVariants(string logName, IEnumerable<AIMoveLogicData> list)
        {
            var log = logName + "\n";
            foreach (var item in list)
            {
                log += item + "\n";
            }
            UnityEngine.Debug.Log(log);
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
                        }
                        threatCount += GetThreatCountFor(cell, shifts);
                    }
                }
            }
            data.PossibleAttackCount = attackCount;
            data.PossibleThreatCount = threatCount;
        }

        private int GetThreatCountFor(CellModel cellMoveFrom, (int,int) shift)
        {
            var result = 0;

            result += GetEnemyNeighboursFor(cellMoveFrom);

            if (GetCellWithShift(cellMoveFrom, shift, out var neighbourCell, -1))
            {
                if (neighbourCell.Chip == null)
                    result += GetEnemyNeighboursFor(neighbourCell);
            }

            return result;
        }

        private int GetEnemyNeighboursFor(CellModel cell)
        {
            var result = 0;
            foreach (var shifts in _gameRules.MoveVariants)
            {
                if (GetCellWithShift(cell, shifts, out var possibleEnemyCell))
                {
                    if (possibleEnemyCell.Chip != null && possibleEnemyCell.Chip.Team != _currentTeam)
                        result++;
                }
            }
            return result;
        }

        private bool GetCellWithShift(CellModel cell, (int, int) shift, out CellModel resultCell, int shiftMultiplier = 1)
        {
            var shiftedRow = cell.RowColPair.Row + shift.Item1 * shiftMultiplier;
            var shiftedCol = cell.RowColPair.Col + shift.Item2 * shiftMultiplier;
            return _fieldModel.GetCellInPosition(shiftedRow, shiftedCol, out resultCell);
        }
    }

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
