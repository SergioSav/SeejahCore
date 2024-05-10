using System;

namespace Assets.Scripts.Core.Utils
{
    [Serializable]
    public enum SoundType
    {
        Unknown = 0,
        BGMusic,
        Win,
        Lose,
        Click,
        ToggleSwitch,
        ChipMove
    }
}