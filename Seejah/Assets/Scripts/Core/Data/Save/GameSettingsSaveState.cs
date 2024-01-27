using System;

namespace Assets.Scripts.Core.Data
{
    [Serializable]
    public class GameSettingsSaveState
    {
        public bool NeedUseUltimateAI;
        public bool IsRandomPlacement;
        public bool SoundOn;
        public bool MusicOn;
    }
}
