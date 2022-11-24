using System;

namespace Assets.Scripts.Core.Data
{
    [Serializable]
    public class CustomizationData
    {
        public int Id;
        public string Name;
        public string Description;
        public int ImageId;
        public PriceData PriceId;
    }
}
