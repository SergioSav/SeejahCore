using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.Data.Services
{

    public class SpriteSupplier : ISpriteSupplier
    {
        private readonly Dictionary<int,string> _spriteNames = new Dictionary<int, string>();

        public SpriteSupplier(List<VisualData> visualDataList)
        {
            foreach (var item in visualDataList)
            {
                if (item.Type == AssetType.Sprite)
                    _spriteNames[item.Id] = item.AssetName;
            }
        }

        public Sprite GetSprite(int id)
        {
            return Resources.Load<Sprite>(_spriteNames[id]);
        }
    }
}
