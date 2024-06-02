namespace Assets.Scripts.Core.Data
{
    public static class DropDataExtensions
    {
        public static DropData Merge(this DropData dataLeft, DropData dataRight)
        {
            var result = dataLeft;
            if (dataRight.Currency == dataLeft.Currency)
                result.Value += dataRight.Value;
            return result;
        }
    }
}
