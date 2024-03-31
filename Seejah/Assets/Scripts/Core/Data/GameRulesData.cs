using System;

namespace Assets.Scripts.Core.Data
{
    [Serializable]
    public class GameRulesData
    {
        public int FieldRowCount;
        public int FieldColCount;
        public int ChipPlacementCount;
        public int ChipStartCount;
        public int ChipMoveDistance;
        public int MinimalChipCountInGame;
        public DropData WinDrop;
        public DropData AdsReward;
        public int FullscreenAdsWinLimit;
        public int WinRatingChange;
        public int LoseRatingChange;
        public int LoseStreakLimit;
        public int WinStreakLimit;
        public int UltimateWinStreakLimit;
    }
}
