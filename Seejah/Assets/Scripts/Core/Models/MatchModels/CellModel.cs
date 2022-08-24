namespace Assets.Scripts.Core.Models
{
    public class CellModel
    {
        private RowColPair _rowColPair;
        private ChipModel _chip;

        public RowColPair RowColPair => _rowColPair;
        public ChipModel Chip => _chip;

        public CellModel(int row, int col)
        {
            _rowColPair = new RowColPair();
            _rowColPair.Row = row;
            _rowColPair.Col = col;
        }

        public void SetChip(ChipModel chip)
        {
            _chip = chip;
        }
    }
}
