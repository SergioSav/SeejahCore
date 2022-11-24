using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = nameof(CustomizationDataScriptableObject), menuName = "Data/Create SO/Customization Data", order = 2)]
    public class CustomizationDataScriptableObject : ScriptableObject
    {
        public List<CustomizationData> CustomizationDataList;
    }
}
