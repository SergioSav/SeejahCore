using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Core.Data.Services
{

    public class PrefabPrototypeSupplier : IPrefabPrototypeSupplier
    {
        private readonly Dictionary<int, string> _prototypesName = new Dictionary<int, string>();
        private ConfigsStorage _configsStorage;

        public PrefabPrototypeSupplier(ConfigsStorage configStorage,  List<VisualData> visualDataList)
        {
            _configsStorage = configStorage;
            foreach (var item in visualDataList)
            {
                if (item.Type == AssetType.Prefab)
                    _prototypesName[item.Id] = item.AssetName;
            }
        }

        public T GetPrototype<T>(int id) where T : Object
        {
            var data = _configsStorage.CustomizationDataList.Where(v => v.Id == id).FirstOrDefault();
            return Resources.Load<T>(_prototypesName[data.PrefabId]);
        }
    }
}
