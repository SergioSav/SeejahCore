using Assets.Scripts.Core.Data.Services;

namespace Assets.Scripts.Core.Data
{
    public interface IGameSettingsSetup : IGameSettings
    {
        void SetRandomPlacementPhase(bool value);
        void SetUsingUltimateAI(bool value);
        void SaveChanges();
    }

    public interface IGameSettings
    {
        bool IsRandomPlacementPhase { get; }
        bool NeedUseUltimateAI { get; }
    }

    public class GameSettings : IGameSettingsSetup
    {
        private bool _isRandomPlacementPhase;
        private bool _needUseUltimateAI;
        private readonly ISaveService _saveService;

        public bool IsRandomPlacementPhase => _isRandomPlacementPhase;
        public bool NeedUseUltimateAI => _needUseUltimateAI;

        public GameSettings(ISaveService saveService)
        {
            _saveService = saveService;
            var saveState = _saveService.Load();
            if (saveState != null)
            {
                _isRandomPlacementPhase = saveState.GameSettingsSave.IsRandomPlacement;
                _needUseUltimateAI = saveState.GameSettingsSave.NeedUseUltimateAI;
            }
        }

        public void SetRandomPlacementPhase(bool value)
        {
            _isRandomPlacementPhase = value;
        }

        public void SetUsingUltimateAI(bool value)
        {
            _needUseUltimateAI = value;
        }

        public void SaveChanges()
        {
            var saveState = new GameSettingsSaveState
            {
                NeedUseUltimateAI = _needUseUltimateAI,
                IsRandomPlacement = _isRandomPlacementPhase
            };
            _saveService.Save(saveState);
        }
    }
}
