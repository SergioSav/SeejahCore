using System;
using System.Collections.Generic;

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
        public int SelectedChipColorId;
        public int SelectedBoardId;
        public int SelectedFloorId;
        public List<int> UnlockedCustomizationItems;
        public GameSettingsSaveState GameSettingsSave;
        public int CurrentGold;
        public bool WasTutorialShown;

        public UserSaveState()
        {
            GameSettingsSave = new GameSettingsSaveState();
        }
    }
}