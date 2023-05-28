using UnityEngine;

namespace Assets.Scripts.Core.Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = nameof(ColorConfigScriptableObject), menuName = "Data/Create SO/Color config", order = 3)]
    public class ColorConfigScriptableObject : ScriptableObject
    {
        public Color Team1Color;
        public Color Team2Color;
    }
}
