namespace Assets.Scripts.Core.Models
{
    public interface IPlayerModel
    {
        TeamType TeamType { get; }
        bool IsHuman { get; }
        bool ReadyForBattle { get; }

        void Setup(TeamType team, IBrain brain);
        void AddChipInGame();
        void RemoveInGameChip();
        bool HasEnoughChipInGame();
        void MakeTurn();
        void SelectCell(RowColPair rcp);
        void EndTurn();
    }
}