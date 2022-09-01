using Assets.Scripts.Core.Rules;

namespace Assets.Scripts.Core.Models
{
    public class PlayerModel
    {
        private TeamType _teamType;
        private GameRules _gameRules;
        private int _chipsInGame;

        public TeamType TeamType => _teamType;

        public PlayerModel(TeamType teamType, GameRules gameRules)
        {
            _teamType = teamType;
            _gameRules = gameRules;
        }

        public void AddChipInGame()
        {
            _chipsInGame++;
        }

        public bool HasEnoughChipInGame()
        {
            return _chipsInGame >= _gameRules.MinimalChipCountInGame;
        }
    }
}