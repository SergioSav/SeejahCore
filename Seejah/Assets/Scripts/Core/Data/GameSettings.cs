using Assets.Scripts.Core.Data.Services;

namespace Assets.Scripts.Core.Data
{
    public interface IGameSettingsSetup : IGameSettings
    {
        void SaveChanges();
        void SetRandomPlacementPhase(bool isRandomPlacement);
    }

    public interface IGameSettings
    {
        bool IsRandomPlacement { get; }
    }

    public class GameSettings : IGameSettingsSetup
    {
        private readonly ISaveService _saveService;

        public bool IsRandomPlacement { get; private set; }

        public GameSettings(ISaveService saveService)
        {
            _saveService = saveService;
            var saveState = _saveService.Load();
            if (saveState != null)
            {
                IsRandomPlacement = saveState.GameSettingsSave.IsRandomPlacement;
            }
        }

        public void SetRandomPlacementPhase(bool value)
        {
            IsRandomPlacement = value;
        }

        public void SaveChanges()
        {
            var saveState = _saveService.GetSettingsSave();
            saveState.IsRandomPlacement = IsRandomPlacement;
            _saveService.Save(saveState);
        }
    }
}
