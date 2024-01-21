using Assets.Scripts.Core.Framework;
using Assets.Scripts.Core.Rules;
using Assets.Scripts.Core.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Core.Models.AILogic
{
    public class AIAdaptiveBattleLogic : DisposableContainer, ILogic
    {
        private readonly GameRules _gameRules;
        private readonly FieldModel _fieldModel;
        private readonly RandomProvider _random;
        private TeamType _currentTeam;
        private CellModel _selectedCell;
        private CellModel _cellForMove;
        private AIBrainDifficulty _difficulty;

        public AIAdaptiveBattleLogic(GameRules gameRules, FieldModel fieldModel, RandomProvider random, TeamType teamType)
        {
            UnityEngine.Debug.Log("- = ADAPTIVE = -");
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

        public void SetupDifficulty(AIBrainDifficulty difficulty)
        {
            _difficulty = difficulty;
            UnityEngine.Debug.Log($"- = {_difficulty} = -");
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

            AIMoveLogicData variantWithPossibleAttack = GetVariantWithPossibleAttack(variantsWithPossibleMoveFromCells);

            if (variantWithPossibleAttack != default)
            {
                DefineCells(variantWithPossibleAttack);
            }
            else
            {
                var variantWithoutThreats = GetVariantWithoutThreat(variantsWithPossibleMoveFromCells);
                if (variantWithoutThreats != default)
                {
                    DefineCells(variantWithoutThreats);
                }
                else
                {
                    var variantsWithThreat = GetVariantWithThreat(variantsWithPossibleMoveFromCells);
                    if (variantsWithThreat != default)
                    {
                        DefineCells(variantsWithThreat);
                    }
                    else
                    {
                        var variant = variantsWithPossibleMoveFromCells.Random(_random);
                        DefineCells(variant);
                    }
                }
            }
        }

        private AIMoveLogicData GetVariantWithPossibleAttack(IEnumerable<AIMoveLogicData> variantsWithPossibleMoveFromCells)
        {
            AIMoveLogicData variantWithPossibleAttack;
            var variants = variantsWithPossibleMoveFromCells
                .Where(v => v.PossibleAttackCount > 0);
            if (_difficulty >= AIBrainDifficulty.Ultimate)
            {
                variants.OrderByDescending(v => v.PossibleAttackCount)
                    .OrderBy(v => v.PossibleThreatCount);
                LogVariants("PossibleAttack ULT ordered desc", variants);
                variantWithPossibleAttack = variants.FirstOrDefault();
            }
            else if (_difficulty >= AIBrainDifficulty.Hard)
            {
                variants.OrderByDescending(v => v.PossibleAttackCount);
                LogVariants("PossibleAttack hard ordered desc", variants);
                _random.GetRandom(variants, out variantWithPossibleAttack);
            }
            else
            {
                LogVariants("PossibleAttack for random", variants);
                _random.GetRandom(variants, out variantWithPossibleAttack);
            }
            return variantWithPossibleAttack;
        }

        private AIMoveLogicData GetVariantWithoutThreat(IEnumerable<AIMoveLogicData> variantsWithPossibleMoveFromCells)
        {
            IEnumerable<AIMoveLogicData> variants;
            if (_difficulty >= AIBrainDifficulty.Medium)
            {
                variants = variantsWithPossibleMoveFromCells
                    .Where(v => v.PossibleThreatCount == 0);
            }
            else
            {
                variants = variantsWithPossibleMoveFromCells
                    .OrderBy(v => v.PossibleThreatCount);
            }

            _random.GetRandom(variants, out AIMoveLogicData variantWithoutThreat);
            return variantWithoutThreat;
        }

        private AIMoveLogicData GetVariantWithThreat(IEnumerable<AIMoveLogicData> variantsWithPossibleMoveFromCells)
        {
            AIMoveLogicData variant;
            var variants = variantsWithPossibleMoveFromCells
                .OrderBy(v => v.PossibleThreatCount);
            LogVariants("PossibleThreat", variants);

            if (_difficulty >= AIBrainDifficulty.Hard)
                variant = variants.FirstOrDefault();
            else
                _random.GetRandom(variants, out variant);

            return variant;
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
}
