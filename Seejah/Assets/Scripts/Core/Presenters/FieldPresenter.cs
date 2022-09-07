using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.Rules;
using VContainer;
using UniRx;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Core.Views;
using System;
using System.Transactions;
using System.Drawing;
using System.Security.Cryptography;

namespace Assets.Scripts.Core.Presenters
{
    public class FieldPresenter : MonoBehPresenter
    {
        private const int CellSize = 1;

        private GameRules _gameRules;
        private FieldModel _fieldModel;
        private MatchModel _matchModel;
        private Func<CellView, Transform, CellView> _cellViewFactory;
        private Func<ChipView, Transform, ChipView> _chipViewFactory;
        private Dictionary<CellModel, CellView> _cellViews;
        private Dictionary<CellModel, ChipView> _chipViews;

        [SerializeField] private CellView cellPrototype;
        [SerializeField] private ChipView chipPrototype;

        [Inject]
        public void Construct(GameRules gameRules, FieldModel fieldModel, MatchModel matchModel, 
            Func<CellView, Transform, CellView> cellViewFactory,
            Func<ChipView, Transform, ChipView> chipViewFactory
            )
        {
            _gameRules = gameRules;
            _fieldModel = fieldModel;
            _matchModel = matchModel;
            _cellViewFactory = cellViewFactory;
            _chipViewFactory = chipViewFactory;

            _cellViews = new Dictionary<CellModel, CellView>();
            _chipViews = new Dictionary<CellModel, ChipView>();
        }

        private void Start()
        {
            AddForDispose(_fieldModel.UpdateCells.Subscribe(OnCellsUpdate));
            AddForDispose(_fieldModel.AddChip.Subscribe(OnChipAddFor));
            AddForDispose(_fieldModel.MoveChip.Subscribe(OnChipMove));
            AddForDispose(_fieldModel.RemoveChip.Subscribe(OnChipRemove));
            AddForDispose(_fieldModel.CellForTurn.Subscribe(OnChooseCellForTurn));

            _fieldModel.CreateField(_gameRules.RowCount, _gameRules.ColCount);

            _matchModel.SwitchStateTo(MatchStateType.WaitNextPlayer);
            // debug switch, must be after show turn panel
            _matchModel.SwitchStateTo(MatchStateType.WaitPlaceChip);
        }

        private void OnChooseCellForTurn(CellModel cell)
        {
            var currentPlayer = _matchModel.CurrentPlayer.Value;
            if (_matchModel.CurrentState.Value == MatchStateType.WaitPlaceChip)
            {
                _fieldModel.TryGenerateChip(cell.RowColPair.Row, cell.RowColPair.Col, currentPlayer.TeamType);
            }
            else if (_matchModel.CurrentState.Value == MatchStateType.WaitChipMove)
            {
                if (_fieldModel.SelectedCell != null)
                {
                    var isMoveSuccess = _fieldModel.TryMoveSelectedChip(cell.RowColPair.Row, cell.RowColPair.Col);
                    if (isMoveSuccess)
                    {
                        var updatingCells = _fieldModel.DetermineOpponentsChipForRemove(cell.RowColPair.Row, cell.RowColPair.Col, currentPlayer.TeamType);
                        HandleChipRemove(updatingCells);
                        _fieldModel.PrintFieldForDebug();
                        _matchModel.HandleEndTurn();
                    }
                    else
                    {
                        _fieldModel.TrySelectChip(cell.RowColPair.Row, cell.RowColPair.Col, currentPlayer.TeamType);
                    }
                }
                else
                {
                    _fieldModel.TrySelectChip(cell.RowColPair.Row, cell.RowColPair.Col, currentPlayer.TeamType);
                }
            }
        }

        private void OnChipRemove(CellModel cell)
        {
            if (cell == null) return;

            Destroy(_chipViews[cell].gameObject);
            _chipViews[cell] = null;
        }

        private void OnCellsUpdate(List<CellModel> cells)
        {
            // TODO:
            if (cells == null || cells.Count == 0)
                return;

            if (_cellViews.Count == 0)
            {
                GenerateField(cells);
            }
        }

        private void OnChipAddFor(CellModel cell)
        {
            if (cell == null)
                return;

            var pos = new Vector3(cell.RowColPair.Row * CellSize, 0, cell.RowColPair.Col * CellSize);
            var chip = _chipViewFactory.Invoke(chipPrototype, transform);
            chip.Setup(_matchModel.CurrentPlayer.Value.TeamType);
            chip.UpdatePos(pos);
            _chipViews[cell] = chip;

            _matchModel.HandleChipPlace();
        }

        private void OnChipMove(CellModel newCell)
        {
            if (newCell == null)
                return;

            var chipView = _chipViews[_fieldModel.SelectedCell];
            var pos = new Vector3(newCell.RowColPair.Row * CellSize, 0, newCell.RowColPair.Col * CellSize);
            chipView.UpdatePos(pos);
            _chipViews[_fieldModel.SelectedCell] = null;
            _chipViews[newCell] = chipView;
        }

        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                var currentPlayer = _matchModel.CurrentPlayer.Value;
                if (!currentPlayer.IsHuman)
                    return;

                var point = GetNormalizedPosition(GetFieldPoint(Input.mousePosition));
                var rcp = GetRowColPairByPosition(point);
                currentPlayer.SelectCell(rcp);
                Debug.Log(point);
            }
        }

        private void HandleChipRemove(List<CellModel> cells)
        {
            foreach (var cell in cells)
            {
                _matchModel.HandleRemoveChip(cell.Chip.Team);
                _fieldModel.HandleRemoveChip(cell);
            }
        }

        private void GenerateField(List<CellModel> cells)
        {
            foreach (var cell in cells)
            {
                var pos = new Vector3(cell.RowColPair.Row * CellSize, 0, cell.RowColPair.Col * CellSize);
                var cellView = _cellViewFactory.Invoke(cellPrototype, transform);
                cellView.Setup(cell, pos);
                _cellViews[cell] = cellView;
            }
        }

        private Vector3 GetFieldPoint(Vector3 mouseInputPos)
        {
            var mousePos = mouseInputPos;
            mousePos.z = Camera.main.transform.position.y - transform.position.y;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            return mousePos - transform.position;
        }

        private Vector3 GetNormalizedPosition(float xPos, float zPos)
        {
            var cellXPos = Mathf.Floor(Mathf.Abs(xPos / CellSize));
            var cellZPos = Mathf.Floor(Mathf.Abs(zPos / CellSize));
            return new Vector3(cellXPos, 0, cellZPos);
        }

        private Vector3 GetNormalizedPosition(Vector3 pos)
        {
            return GetNormalizedPosition(pos.x, pos.z);
        }

        private RowColPair GetRowColPairByPosition(Vector3 pos)
        {
            foreach (var kvp in _cellViews)
            {
                if (kvp.Value.Position == pos)
                    return kvp.Key.RowColPair;
            }
            return default;
        }
    }
}
