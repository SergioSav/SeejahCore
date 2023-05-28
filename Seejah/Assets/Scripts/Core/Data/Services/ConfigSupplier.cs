using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.Data.Services
{

    public class ConfigSupplier : IConfigSupplier
    {
        private readonly Dictionary<int, string> _configNames = new Dictionary<int, string>();

        public ConfigSupplier(List<VisualData> visualDataList)
        {
            foreach (var item in visualDataList)
            {
                if (item.Type == AssetType.SOConfig)
                    _configNames[item.Id] = item.AssetName;
            }
        }

        public T GetConfig<T>(int id) where T : Object
        {
            return Resources.Load<T>(_configNames[id]);
        }
    }
}
