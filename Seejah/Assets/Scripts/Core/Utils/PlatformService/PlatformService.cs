using Assets.Scripts.Core.Data;
using Assets.Scripts.Core.Rules;
using System;
using YG;

namespace Assets.Scripts.Core.Utils
{
    public class PlatformService
    {
        private const int _REWARD_ADS_ID = 1;
        private const string _LEADERBOARD_TECH_NAME = "LBoardTest";

        public event Action<DropData> RewardAdsSeen;

        private GameRules _gameRules;
        private readonly ITimeService _timeService;
        private int _winCount;

        public PlatformService(GameRules gameRules, ITimeService timeService)
        {
            _gameRules = gameRules;
            _timeService = timeService;
            Init();
        }

        public void TryShowFullscreenAds()
        {
            if (_winCount >= _gameRules.FullscreenAdsWinLimit)
            {
                _winCount = 0;
                _timeService.Wait(1)
                    .Then(YandexGame.FullscreenShow);
            }
            else
            {
                _winCount ++;
            }
        }

        public void ShowRewardsAds()
        {
            YandexGame.RewVideoShow(_REWARD_ADS_ID);
        }

        public void ApplyRatingScore(int score)
        {
            YandexGame.NewLeaderboardScores(_LEADERBOARD_TECH_NAME, score);
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
