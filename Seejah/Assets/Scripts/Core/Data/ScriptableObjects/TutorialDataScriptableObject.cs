using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "TutorialDataScriptableObject", menuName = "Data/Create SO/Tutorial Data", order = 5)]
    public class TutorialDataScriptableObject : ScriptableObject
    {
        public List<TutorialData> TutorialDataList;
    }
}
