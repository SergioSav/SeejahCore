using System;

namespace Assets.Scripts.Core.Data.Services
{
    [Serializable]
    public class UserSaveState
    {
        public GameSettingsSaveState GameSettingsSave;
        public PlayerSaveState PlayerSaveState;

        public UserSaveState()
        {
            GameSettingsSave = new GameSettingsSaveState();
            PlayerSaveState = new PlayerSaveState();
        }
    }
}