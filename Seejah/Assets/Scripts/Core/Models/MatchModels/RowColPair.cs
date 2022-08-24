namespace Assets.Scripts.Core.Models
{
    public class RowColPair
    {
        public int Row { get; set; }
        public int Col { get; set; }

        public bool IsEqual(int row, int col)
        {
            return Row == row && Col == col;
        }

        public override bool Equals(object obj)
        {
            if (obj is RowColPair pair)
                return Row == pair.Row && Col == pair.Col;
            return false;
        }

        public override int GetHashCode()
        {
            return Row + Col * 100; // TODO:
        }

        public override string ToString()
        {
            return $"({Row}:{Col})";
        }
    }
}
