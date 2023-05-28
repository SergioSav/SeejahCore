using System;

namespace Assets.Scripts.Core.Data
{
    [Serializable]
    public class CustomizationData
    {
        public string Name;
        public int Id;
        public CustomizationType Type;
        public string Description;
        public int ImageId;
        public int PrefabId;
        public PriceData Price;
    }
}
