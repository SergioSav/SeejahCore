using UnityEngine;

namespace Assets.Scripts.Core.Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameRulesDataScriptableObject", menuName = "Data/Create SO/Game Rules Data", order = 2)]
    public class GameRulesDataScriptableObject : ScriptableObject
    {
        public GameRulesData rulesData;
    }
}
