using System;

namespace Assets.Scripts.Core.Data.Services
{
    [Serializable]
    public class UserSaveState
    {
        public string Name;
        public int Id;
        public int WinCount;
        public int WinStreak;
        public int LoseCount;
        public int LoseStreak;
        public int SelectedChipId;
    }
}