namespace Assets.Scripts.Core.Data
{
    public interface IGameSettingsSetup
    {
        void SetRandomPlacementPhase(bool value);
        void SetUsingUltimateAI(bool value);
    }

    public interface IGameSettings
    {
        bool IsRandomPlacementPhase { get; }
        bool NeedUseUltimateAI { get; }
    }

    public class GameSettings : IGameSettings, IGameSettingsSetup
    {
        private bool _isRandomPlacementPhase;
        private bool _needUseUltimateAI;

        public bool IsRandomPlacementPhase => _isRandomPlacementPhase;
        public bool NeedUseUltimateAI => _needUseUltimateAI;

        public void SetRandomPlacementPhase(bool value)
        {
            _isRandomPlacementPhase = value;
        }

        public void SetUsingUltimateAI(bool value)
        {
            _needUseUltimateAI = value;
        }
    }
}
