using Assets.Scripts.Core.Data.Services;
using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.Rules;
using Assets.Scripts.Core.Views;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private GameRules _gameRules;
        private Func<CellView, Transform, CellView> _cellViewFactory;
        private Func<ChipView, Transform, ChipView> _chipViewFactory;
        private IPrefabPrototypeSupplier _prototypeSupplier;
        private IMaterialSupplier _materialSupplier;
        private ConfigsStorage _configStorage;
        private Dictionary<CellModel, CellView> _cellViews;
        private Dictionary<CellModel, ChipView> _chipViews;

        [SerializeField] private CellView cellPrototype;
        [SerializeField] private GameObject board;
        [SerializeField] private Renderer boardRenderer;
        [SerializeField] private Renderer floorRenderer;
        [SerializeField] private Transform chipSelectPlace1;
        [SerializeField] private Transform chipSelectPlace2;
        [SerializeField] private GameObject chipSelectContainer;

        private Queue<ChipView> _firstTeamChips;
        private Queue<ChipView> _secondTeamChips;
        private List<ChipView> _selectionChips;
        private ChipView _selectedChip;
        private int _chipPrefabId;

        [Inject]
        public void Construct(FieldModel fieldModel, MatchModel matchModel, GameRules gameRules,
                Func<CellView, Transform, CellView> cellViewFactory,
                Func<ChipView, Transform, ChipView> chipViewFactory,
                IPrefabPrototypeSupplier prototypeSupplier,
                IMaterialSupplier materialSupplier, ConfigsStorage configsStorage
                )
        {
            _fieldModel = fieldModel;
            _matchModel = matchModel;
            _gameRules = gameRules;
            _cellViewFactory = cellViewFactory;
            _chipViewFactory = chipViewFactory;
            _prototypeSupplier = prototypeSupplier;
            _materialSupplier = materialSupplier;
            _configStorage = configsStorage;

            _cellViews = new Dictionary<CellModel, CellView>();
            _chipViews = new Dictionary<CellModel, ChipView>();
        }

        private void Start()
        {
            var floorPrefabId = _configStorage.CustomizationDataList.FirstOrDefault(d => d.Id == _matchModel.Options.FloorId).PrefabId;
            floorRenderer.material = _materialSupplier.GetMaterial(floorPrefabId);
            board.SetActive(false);

            _chipPrefabId = _configStorage.CustomizationDataList.FirstOrDefault(d => d.Id == _matchModel.Options.ChipId).PrefabId;

            GenerateChipsForSelect();

            AddForDispose(_fieldModel.UpdateCells.Subscribe(OnCellsUpdate));
            AddForDispose(_fieldModel.SelectCell.Subscribe(OnCellSelect));
            AddForDispose(_fieldModel.AddChip.Subscribe(OnChipAddFor));
            AddForDispose(_fieldModel.MoveChip.Subscribe(OnChipMove));
            AddForDispose(_fieldModel.AttackChip.Subscribe(OnChipAttack));

            AddForDispose(_fieldModel.StartMatch.Subscribe(_ => OnStartMatch()));
        }
        
        private void GenerateChipsForSelect()
        {
            _selectionChips = new List<ChipView>();
            var prototype = _prototypeSupplier.GetPrototype<ChipView>(_chipPrefabId);
            CreateChipForSelect(prototype, chipSelectPlace1, TeamType.FirstTeam);
            CreateChipForSelect(prototype, chipSelectPlace2, TeamType.SecondTeam);
        }

        private void CreateChipForSelect(ChipView prototype, Transform place, TeamType team)
        {
            ChipView chip = _chipViewFactory.Invoke(prototype, place);
            chip.transform.localPosition = Vector3.one * -0.5f;
            chip.Setup(team);
            chip.SetInfiniteRotate();
            _selectionChips.Add(chip);
        }

        private void DestroySelectionChips()
        {
            foreach (var chip in _selectionChips)
                Destroy(chip.gameObject);
        }

        private void OnStartMatch()
        {
            DestroySelectionChips();
            board.SetActive(true);
            var boardPrefabId = _configStorage.CustomizationDataList.FirstOrDefault(d => d.Id == _matchModel.Options.BoardId).PrefabId;
            boardRenderer.material = _materialSupplier.GetMaterial(boardPrefabId);

            GenerateChips();
        }

        private void GenerateChips()
        {
            _firstTeamChips = GenerateChipsForTeam(TeamType.FirstTeam);
            _secondTeamChips = GenerateChipsForTeam(TeamType.SecondTeam);
        }

        private Queue<ChipView> GenerateChipsForTeam(TeamType team)
        {
            var resultList = new Queue<ChipView>();
            for (int i = 0; i < _fieldModel.ChipCountForOnePlayer; i++)
            {
                var prototype = _prototypeSupplier.GetPrototype<ChipView>(_chipPrefabId);
                var chip = _chipViewFactory.Invoke(prototype, transform);
                chip.Setup(team);
                chip.PlaceOutBoard(GetChipPosForPlacement(team, i));
                resultList.Enqueue(chip);
            }
            return resultList;
        }

        private Vector3 GetChipPosForPlacement(TeamType team, int index)
        {
            var fieldWidth = CellSize * _gameRules.ColCount;
            var fieldHeight = CellSize * _gameRules.RowCount;
            var xPos = team == TeamType.FirstTeam ? -1f : fieldWidth / ChipView.PlacementPhaseScale;
            var zPos = fieldHeight / ChipView.PlacementPhaseScale - 1f - index;
            return new Vector3(xPos, 0, zPos);
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
            var chip = _matchModel.ActivePlayer.TeamType == TeamType.FirstTeam ? _firstTeamChips.Dequeue() : _secondTeamChips.Dequeue();
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

        private void OnCellSelect(CellModel cell)
        {
            if (_selectedChip)
            {
                _selectedChip.SetSelected(false);
                _selectedChip = null;
            }
            if (cell != null)
            {
                _selectedChip = _chipViews[cell];
                _selectedChip.SetSelected(true);
            }
        }

        private void Update()
        {
            if (!_matchModel.CanInteract)
                return;
            if (Input.GetMouseButtonUp(0))
            {
                var currentPlayer = _matchModel.ActivePlayer;
                if (!currentPlayer.IsHuman)
                    return;

                var point = GetNormalizedPosition(GetFieldPoint(Input.mousePosition));
                var rcp = GetRowColPairByPosition(point);
                if (rcp != null)
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
