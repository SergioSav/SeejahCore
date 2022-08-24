using Assets.Scripts.Core.Models;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Core.Views
{
    public class CellView : MonoBehaviour, ICellView
    {
        [SerializeField] private TextMeshPro text;
        private CellModel _cell;
        private Vector3 _pos;

        public void Setup(CellModel cell, Vector3 pos)
        {
            _cell = cell;
            _pos = pos;
            transform.position = pos;
            text.text = $"({_cell.RowColPair.Row},{_cell.RowColPair.Col})";
        }

        public Vector3 Position => _pos;
    }
}