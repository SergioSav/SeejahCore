namespace Assets.Scripts.Core.Models
{
    public interface IAIBrain : IBrain
    {
        void TuneDifficulty(int winStreak, int loseStreak);
        void Reset();
        void SwitchToBattle();
        void SwitchToPlacement();
        bool TryGetCellForSelect(out CellModel cell);
        bool TryGetCellForMove(out CellModel cell);
        void ResetLogic();
    }
}