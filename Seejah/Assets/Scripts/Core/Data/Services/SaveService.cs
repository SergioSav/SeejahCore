using UnityEngine;

namespace Assets.Scripts.Core.Data.Services
{
    public interface ISaveService
    {
        UserSaveState Load();
        void Save(UserSaveState newSave);
    }

    public class SaveService : ISaveService
    {
        private const string SAVE_NAME = "USS";

        private UserSaveState _saveState;
        private IDataSerializer _dataSerializer;

        public SaveService(IDataSerializer dataSerializer)
        {
            _dataSerializer = dataSerializer;
        }

        public UserSaveState Load()
        {
            var savedString = PlayerPrefs.GetString(SAVE_NAME);
            if (savedString != null)
            {
                _saveState = _dataSerializer.DeserializeTo<UserSaveState>(savedString);
            }
            return _saveState;
        }

        public void Save(UserSaveState newSave)
        {
            if (_saveState == newSave)
                return;

            _saveState = newSave;
            var saveString = _dataSerializer.SerializeFrom(_saveState);
            PlayerPrefs.SetString(SAVE_NAME, saveString);
            PlayerPrefs.Save();
        }
    }
}