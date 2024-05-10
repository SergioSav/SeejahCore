using Assets.Scripts.Core.Data;
using Assets.Scripts.Core.Rules;
using System;
using System.Collections.Generic;
using YG;

namespace Assets.Scripts.Core.Utils
{
    public class YandexGamesPlatformService : IPlatformService
    {
        private const int _REWARD_ADS_ID = 1;
        private const string _LEADERBOARD_TECH_NAME = "LBoardTest";

        public event Action<DropData> RewardAdsSeen;

        private GameRules _gameRules;
        private readonly ITimeService _timeService;
        private int _winCount;

        public YandexGamesPlatformService(GameRules gameRules, ITimeService timeService)
        {
            _gameRules = gameRules;
            _timeService = timeService;
            Init();
        }

        public bool TryShowFullscreenAds()
        {
            if (_winCount >= _gameRules.FullscreenAdsWinLimit)
            {
                _winCount = 0;
                _timeService.Wait(1)
                    .Then(YandexGame.FullscreenShow);
                return true;
            }
            _winCount ++;
            return false;
        }

        public bool TryPromptShow()
        {
            if (YandexGame.SDKEnabled && YandexGame.EnvironmentData.promptCanShow)
            {
                YandexGame.PromptShow();
                return true;
            }
            return false;
        }

        public bool TryShowReview()
        {
            if (YandexGame.SDKEnabled && YandexGame.EnvironmentData.reviewCanShow)
            {
                YandexGame.ReviewShow(true);
                return true;
            }
            return false;
        }

        public void ShowRewardsAds()
        {
            YandexGame.RewVideoShow(_REWARD_ADS_ID);
        }

        public void ApplyRatingScore(int score)
        {
            YandexGame.NewLeaderboardScores(_LEADERBOARD_TECH_NAME, score);
        }

        public string LoadData()
        {
            if (YandexGame.SDKEnabled)
                return YandexGame.savesData.SaveString;
            return null;
        }

        public void SaveData(string saveString)
        {
            YandexGame.savesData.SaveString = saveString;
            YandexGame.SaveProgress();
        }

        public string GetLanguageId()
        {
            return YandexGame.EnvironmentData.language;
        }

        public void SendMetric(string eventName)
        {
            YandexMetrica.Send(eventName);
        }

        public void SendMetric(string eventName, IDictionary<string, string> eventParams)
        {
            YandexMetrica.Send(eventName, eventParams);
        }

        private void Init()
        {
            YandexGame.RewardVideoEvent += OnRewardVideoEnd;
        }

        private void OnRewardVideoEnd(int id)
        {
            if (id == _REWARD_ADS_ID)
                RewardAdsSeen?.Invoke(_gameRules.AdsReward);
        }
    }
}
