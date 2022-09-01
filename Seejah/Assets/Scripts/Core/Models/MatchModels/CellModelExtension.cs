using System;

namespace Assets.Scripts.Core.Models
{
    public static class CellModelExtension
    {
        public static bool TestDistanceToCell(this CellModel cell, CellModel targetCell, int distance)
        {
            return Math.Abs(cell.RowColPair.Row - targetCell.RowColPair.Row) <= distance && 
                Math.Abs(cell.RowColPair.Col - targetCell.RowColPair.Col) <= distance;
        }
    }
}
