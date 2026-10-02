using Assets.Scripts.Core.Data;
using Assets.Scripts.Core.Rules;
using Assets.Scripts.Core.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.SceneInstallers
{
    public class DummyPlatformService : IPlatformService
    {
        private readonly GameRules _gameRules;
        private readonly string _className;

        public DummyPlatformService(GameRules gameRules)
        {
            _gameRules = gameRules;

            _className = typeof(DummyPlatformService).Name;
        }

        public event Action<DropData> RewardAdsSeen;

        public void ApplyRatingScore(int score)
        {
            Log($":{nameof(ApplyRatingScore)} => {score}");
        }

        public string GetLanguageId()
        {
            return "en";
        }

        public string LoadData()
        {
            return string.Empty;
        }

        public void SaveData(string saveString)
        {
            Log($":{nameof(SaveData)} => {saveString}");
        }

        public void SendMetric(string eventName)
        {
            Log($":{nameof(SendMetric)} => {eventName}");
        }

        public void SendMetric(string eventName, IDictionary<string, string> eventParams)
        {
            var paramsString = string.Empty;
            foreach (var param in eventParams)
            {
                paramsString += $"{param.Key} - {param.Value}\n";
            }
            Log($":{nameof(SendMetric)} => {eventName} with params {paramsString}");
        }

        public void ShowRewardsAds()
        {
            Log($":{nameof(ShowRewardsAds)}");
            RewardAdsSeen?.Invoke(_gameRules.AdsReward);
        }

        public bool TryPromptShow()
        {
            return false;
        }

        public bool TryShowFullscreenAds()
        {
            return false;
        }

        public bool TryShowReview()
        {
            return false;
        }

        private void Log(string logString)
        {
            Debug.Log($"{_className}:{logString}");
        }
    }
}