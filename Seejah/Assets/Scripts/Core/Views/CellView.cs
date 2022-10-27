using Assets.Scripts.Core.Models;
using System;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Core.Views
{
    public class CellView : MonoBehaviour, ICellView
    {
        [SerializeField] private TextMeshPro text;
        [SerializeField] private Material _blackMat;
        [SerializeField] private Material _centerMat;
        [SerializeField] private Renderer _planeRenderer;
        private CellModel _cell;
        private Vector3 _pos;
        
        public void Setup(CellModel cell, Vector3 pos)
        {
            _cell = cell;
            _pos = pos;
            transform.position = pos;
            text.text = $"({_cell.RowColPair.Row},{_cell.RowColPair.Col})";
            SetupMaterial();
        }

        private void SetupMaterial()
        {
            Material material = _planeRenderer.material;
            if (_cell.IsCentral)
                material = _centerMat;
            else
            {
                if (Math.Abs(_cell.RowColPair.Row - _cell.RowColPair.Col) % 2 > 0)
                    material = _blackMat;
            }
            _planeRenderer.material = material;
        }

        public Vector3 Position => _pos;
    }
}