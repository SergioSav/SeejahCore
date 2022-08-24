using Assets.Scripts.Core.Models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.Views
{
    public class FieldView : MonoBehaviour, IFieldView
    {
        private const int CellSize = 1;

        [SerializeField] private CellView CellPrototype;
        [SerializeField] private ChipView ChipPrototype;

        private ChipView _selectedChip;
        private Action<int, int> _onFieldCellSelect;
        private List<CellModel> _cells;
        private Dictionary<CellModel, CellView> _cellViews;

        public void Start()
        {
            _cellViews = new Dictionary<CellModel, CellView>();
        }

        public void Update()
        {
            Vector3 point = default;
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButtonUp(0))
            {
                point = GetNormalizedPosition(GetFieldPoint(Input.mousePosition));
                var rcp = GetRowColPairByPosition(point);
                _onFieldCellSelect(rcp.Row, rcp.Col);
                Debug.Log($"pos = {point}, rcp = {rcp}");
            }
        }

        public void SetCellSelectCallback(Action<int, int> onFieldCellSelect)
        {
            _onFieldCellSelect = onFieldCellSelect;
        }

        public void UpdateCells(List<CellModel> cells)
        {
            if (_cells == null)
            {
                GenerateField(cells);
            }
            _cells = cells;
            foreach (var cell in cells)
            {
                // TODO: upd cellView by cellModel
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

        private void GenerateField(List<CellModel> cells)
        {
            foreach (var cell in cells)
            { 
                var pos = new Vector3(cell.RowColPair.Row * CellSize, 0, cell.RowColPair.Col * CellSize);
                var cellView = Instantiate(CellPrototype, pos, Quaternion.identity);
                cellView.Setup(cell, pos);
                _cellViews[cell] = cellView;
            }
        }

        public void AddChip(CellModel cell)
        {
            var pos = new Vector3(cell.RowColPair.Row * CellSize, 0, cell.RowColPair.Col * CellSize);
            var chip = Instantiate(ChipPrototype, pos, Quaternion.identity);
        }
    }
}