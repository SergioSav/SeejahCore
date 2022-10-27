using System;

namespace Assets.Scripts.Core.Data
{
    [Serializable]
    public class VisualData
    {
        public int Id;
        public string Name;
        public string Description;
        public string AssetName;
        public AssetType Type;
    }
}
