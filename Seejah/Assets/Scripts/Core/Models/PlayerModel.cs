namespace Assets.Scripts.Core.Models
{
    public class PlayerModel
    {
        private TeamType _teamType;

        public TeamType TeamType => _teamType;

        public PlayerModel(TeamType teamType)
        {
            _teamType = teamType;
        }
    }
}