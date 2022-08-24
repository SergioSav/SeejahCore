namespace Assets.Scripts.Core.Models
{
    public class ChipModel
    {
        public TeamType Team { get; private set; }

        public ChipModel(TeamType team)
        {
            Team = team;
        }
    }
}
