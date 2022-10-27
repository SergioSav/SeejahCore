using Assets.Scripts.Core.Data.Services;
using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.Views;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using VContainer;

namespace Assets.Scripts.Core.Presenters
{
    public class FieldPresenter : MonoBehPresenter
    {
        private const int CellSize = 1;

        private FieldModel _fieldModel;
        private MatchModel _matchModel;
        private Func<CellView, Transform, CellView> _cellViewFactory;
        private Func<ChipView, Transform, ChipView> _chipViewFactory;
        private IPrefabPrototypeSupplier _prototypeSupplier;
        private Dictionary<CellModel, CellView> _cellViews;
        private Dictionary<CellModel, ChipView> _chipViews;

        [SerializeField] private CellView cellPrototype;
        private int _chipId; // TODO: temp, receive from MatchOptions
        private Stack<ChipView> _firstTeamChips;
        private Stack<ChipView> _secondTeamChips;

        [Inject]
        public void Construct(FieldModel fieldModel, MatchModel matchModel, 
            Func<CellView, Transform, CellView> cellViewFactory,
            Func<ChipView, Transform, ChipView> chipViewFactory,
            IPrefabPrototypeSupplier prototypeSupplier
            )
        {
            _fieldModel = fieldModel;
            _matchModel = matchModel;
            _cellViewFactory = cellViewFactory;
            _chipViewFactory = chipViewFactory;
            _prototypeSupplier = prototypeSupplier;

            _cellViews = new Dictionary<CellModel, CellView>();
            _chipViews = new Dictionary<CellModel, ChipView>();
        }

        private void Start()
        {
            _chipId = 1;

            GenerateChips();

            AddForDispose(_fieldModel.UpdateCells.Subscribe(OnCellsUpdate));
            AddForDispose(_fieldModel.AddChip.Subscribe(OnChipAddFor));
            AddForDispose(_fieldModel.MoveChip.Subscribe(OnChipMove));
            AddForDispose(_fieldModel.AttackChip.Subscribe(OnChipAttack));
        }

        private void GenerateChips()
        {
            _firstTeamChips = GenerateChipsForTeam(TeamType.FirstTeam);
            _secondTeamChips = GenerateChipsForTeam(TeamType.SecondTeam);
        }

        private Stack<ChipView> GenerateChipsForTeam(TeamType team)
        {
            var resultList = new Stack<ChipView>();
            for (int i = 0; i < _fieldModel.ChipCountForOnePlayer; i++)
            {
                var prototype = _prototypeSupplier.GetPrototype<ChipView>(_chipId);
                var chip = _chipViewFactory.Invoke(prototype, transform);
                chip.Setup(team);
                var pos = new Vector3(-(int)team, 0, i);
                chip.PlaceOutBoard(pos);
                resultList.Push(chip);
            }
            return resultList;
        }

        private void OnChipAttack(AttackThreesome threesome)
        {
            if (threesome == null) return;
            ShowChipAttack(threesome);
        }

        private void ShowChipAttack(AttackThreesome threesome)
        {
            var firstAttacker = _chipViews[threesome.FirstAttacker];
            var secondAttacker = _chipViews[threesome.SecondAttacker];
            var victim = _chipViews[threesome.Victim];
            firstAttacker.Attack(victim.transform.position);
            secondAttacker.Attack(victim.transform.position);
            victim.RemoveFromBoard();
            _chipViews[threesome.Victim] = null;
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
            var chip = _matchModel.ActivePlayer.TeamType == TeamType.FirstTeam ? _firstTeamChips.Pop() : _secondTeamChips.Pop();
            chip.PlaceOnBoard(pos);
            _chipViews[cell] = chip;
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
                var currentPlayer = _matchModel.ActivePlayer;
                if (!currentPlayer.IsHuman)
                    return;

                var point = GetNormalizedPosition(GetFieldPoint(Input.mousePosition));
                var rcp = GetRowColPairByPosition(point);
                currentPlayer.SelectCell(rcp);
                Debug.Log(point);
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
