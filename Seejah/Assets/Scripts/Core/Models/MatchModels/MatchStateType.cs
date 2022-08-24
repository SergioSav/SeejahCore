namespace Assets.Scripts.Core.Models
{
    public enum MatchStateType
    {
        None = 0,
        WaitPlaceChip = 1,
        WaitNextPlayer = 2,
        ReadyForPlay = 3,
        WaitChipMove = 4,
        WaitNextTurn = 5,
        EndMatch = 6
    }
}
