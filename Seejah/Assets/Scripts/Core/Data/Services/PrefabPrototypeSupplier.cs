using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.Data.Services
{

    public class PrefabPrototypeSupplier : IPrefabPrototypeSupplier
    {
        private readonly Dictionary<int, string> _prototypesName = new Dictionary<int, string>();

        public PrefabPrototypeSupplier(List<VisualData> visualDataList)
        {
            foreach (var item in visualDataList)
            {
                if (item.Type == AssetType.Prefab)
                    _prototypesName[item.Id] = item.AssetName;
            }
        }

        public T GetPrototype<T>(int id) where T : Object
        {
            return Resources.Load<T>(_prototypesName[id]);
        }
    }
}
