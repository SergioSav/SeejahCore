namespace Assets.Scripts.Core.Models
{
    public class CellModel
    {
        private RowColPair _rowColPair;
        private ChipModel _chip;

        public RowColPair RowColPair => _rowColPair;
        public ChipModel Chip => _chip;

        public bool IsCentral { get; private set; }

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

        public void ClearChip()
        {
            _chip = null;
        }

        public void SetCentral()
        {
            IsCentral = true;
        }

        public override string ToString()
        {
            return _rowColPair.ToString();
        }
    }
}
