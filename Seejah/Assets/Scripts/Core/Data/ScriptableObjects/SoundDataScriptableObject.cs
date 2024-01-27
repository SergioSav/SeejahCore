using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "SoundDataScriptableObject", menuName = "Data/Create SO/Sound Data", order = 6)]
    public class SoundDataScriptableObject : ScriptableObject
    {
        public List<SoundData> SoundDataList;
    }
}
