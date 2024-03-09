using System;
using System.Collections.Generic;

namespace Assets.Scripts.Core.Data.Services
{
    [Serializable]
    public class PlayerSaveState
    {
        public int Id = -1;
        public string Name;
        public int WinCount;
        public int WinStreak;
        public int LoseCount;
        public int LoseStreak;
        public int SelectedChipId;
        public int SelectedChipColorId;
        public int SelectedBoardId;
        public int SelectedFloorId;
        public int CurrentGold;
        public bool WasTutorialShown;
        public List<int> UnlockedCustomizationItems;
    }
}