using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.Data.Services
{

    public class MaterialSupplier : IMaterialSupplier
    {
        private readonly Dictionary<int, string> _materialNames = new Dictionary<int, string>();

        public MaterialSupplier(List<VisualData> visualDataList)
        {
            foreach (var item in visualDataList)
            {
                if (item.Type == AssetType.Material)
                    _materialNames[item.Id] = item.AssetName;
            }
        }

        public Material GetMaterial(int id)
        {
            return Resources.Load<Material>(_materialNames[id]);
        }
    }
}
