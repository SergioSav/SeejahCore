using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "VisualDataScriptableObject", menuName = "Data/Create SO/Visual Data", order = 1)]
    public class VisualDataScriptableObject : ScriptableObject
    {
        public List<VisualData> VisualDataList;
    }
}
