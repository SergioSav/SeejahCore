namespace Assets.Scripts.Core.Models
{
    public interface IBrain
    {
        bool IsHuman { get; }

        void Reset();
        void SwitchToBattle();
        void SwitchToPlacement();
        bool TryGetCellForMove(out CellModel cell);
    }
}