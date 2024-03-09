using UnityEngine;

namespace Assets.Scripts.Core.Data.Services
{
    public interface ISaveService
    {
        GameSettingsSaveState GetSettingsSave();
        PlayerSaveState GetPlayerSaveState();
        UserSaveState Load();
        void Save(PlayerSaveState playerState);
        void Save(GameSettingsSaveState settingsSave);
    }

    public class SaveService : ISaveService
    {
        private const string SAVE_NAME = "USS";

        private UserSaveState _currentSaveState;
        private IDataSerializer _dataSerializer;

        public SaveService(IDataSerializer dataSerializer)
        {
            _dataSerializer = dataSerializer;
        }

        public UserSaveState Load()
        {
            if (_currentSaveState != null)
                return _currentSaveState;

            var savedString = PlayerPrefs.GetString(SAVE_NAME);
            if (savedString != null)
                _currentSaveState = _dataSerializer.DeserializeTo<UserSaveState>(savedString);

            _currentSaveState ??= new UserSaveState();

            return _currentSaveState;
        }

        public void Save(PlayerSaveState newSave)
        {
            _currentSaveState.PlayerSaveState = newSave;
            InternalSave(_currentSaveState);
        }

        public void Save(GameSettingsSaveState settingsSave)
        {
            _currentSaveState.GameSettingsSave = settingsSave;
            InternalSave(_currentSaveState);
        }

        public GameSettingsSaveState GetSettingsSave() => _currentSaveState.GameSettingsSave;

        public PlayerSaveState GetPlayerSaveState() => _currentSaveState.PlayerSaveState;

        private void InternalSave(UserSaveState newSave)
        {
            _currentSaveState = newSave;
            var saveString = _dataSerializer.SerializeFrom(_currentSaveState);
            PlayerPrefs.SetString(SAVE_NAME, saveString);
            PlayerPrefs.Save();
        }
    }
}