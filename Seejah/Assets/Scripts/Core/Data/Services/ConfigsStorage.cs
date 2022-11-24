using Assets.Scripts.Core.Data.ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.Data.Services
{
    public class ConfigsStorage
    {
        public List<VisualData> VisualDataList => _visualDataSO.VisualDataList;
        public List<CustomizationData> CustomizationDataList=> _customizationDataSO.CustomizationDataList;
        public GameRulesData GameRulesData => _gameRulesDataSO.rulesData;

        private GameRulesDataScriptableObject _gameRulesDataSO;
        private VisualDataScriptableObject _visualDataSO;
        private CustomizationDataScriptableObject _customizationDataSO;

        public ConfigsStorage()
        {
            LoadConfigs();
        }

        private void LoadConfigs()
        {
            _gameRulesDataSO = Resources.Load<GameRulesDataScriptableObject>("GameRulesDataScriptableObject");
            _visualDataSO = Resources.Load<VisualDataScriptableObject>("VisualDataScriptableObject");
            _customizationDataSO = Resources.Load<CustomizationDataScriptableObject>(nameof(CustomizationDataScriptableObject));
        }
    }
}
