namespace Assets.Scripts.Core.Data
{
    public static class DropDataExtensions
    {
        public static DropData Merge(this DropData dataLeft, DropData dataRight)
        {
            if (dataRight.Currency == dataLeft.Currency)
                dataLeft.Value += dataRight.Value;
            return dataLeft;
        }
    }
}
