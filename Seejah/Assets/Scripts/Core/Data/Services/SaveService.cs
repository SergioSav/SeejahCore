using UnityEngine;

namespace Assets.Scripts.Core.Data.Services
{
    public interface ISaveService
    {
        UserSaveState Load();
        void Save(UserSaveState newSave);
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
            {
                _currentSaveState = _dataSerializer.DeserializeTo<UserSaveState>(savedString);
            }
            return _currentSaveState;
        }

        public void Save(UserSaveState newSave)
        {
            if (_currentSaveState == newSave)
                return;

            InternalSave(newSave);
        }

        public void Save(GameSettingsSaveState settingsSave)
        {
            if (_currentSaveState == null)
                return;

            _currentSaveState.GameSettingsSave = settingsSave;
            InternalSave(_currentSaveState);
        }

        private void InternalSave(UserSaveState newSave)
        {
            _currentSaveState = newSave;
            var saveString = _dataSerializer.SerializeFrom(_currentSaveState);
            PlayerPrefs.SetString(SAVE_NAME, saveString);
            PlayerPrefs.Save();
        }
    }
}