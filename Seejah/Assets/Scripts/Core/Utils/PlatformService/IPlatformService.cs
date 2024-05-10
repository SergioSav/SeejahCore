using Assets.Scripts.Core.Data;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Core.Utils
{
    public interface IPlatformService
    {
        event Action<DropData> RewardAdsSeen;

        string GetLanguageId();
        string LoadData();
        void SaveData(string saveString);
        void ApplyRatingScore(int score);
        void ShowRewardsAds();
        bool TryShowFullscreenAds();
        bool TryPromptShow();
        bool TryShowReview();

        void SendMetric(string eventName);
        void SendMetric(string eventName, IDictionary<string, string> eventParams);
    }
}