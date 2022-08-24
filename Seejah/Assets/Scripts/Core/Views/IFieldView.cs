using Assets.Scripts.Core.Models;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Core.Views
{
    public interface IFieldView
    {
        void UpdateCells(List<CellModel> cells);
        void SetCellSelectCallback(Action<int, int> onFieldCellSelect);
        void AddChip(CellModel cell);
    }
}