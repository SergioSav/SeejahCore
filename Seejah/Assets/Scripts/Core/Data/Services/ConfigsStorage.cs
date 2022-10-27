using Assets.Scripts.Core.Data.ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.Data.Services
{
    public class ConfigsStorage
    {
        public List<VisualData> VisualDataList => _visualDataSO.VisualDataList;
        public GameRulesData GameRulesData => _gameRulesDataSO.rulesData;

        private GameRulesDataScriptableObject _gameRulesDataSO;
        private VisualDataScriptableObject _visualDataSO;

        public ConfigsStorage()
        {
            LoadConfigs();
        }

        private void LoadConfigs()
        {
            _gameRulesDataSO = Resources.Load<GameRulesDataScriptableObject>("GameRulesDataScriptableObject");
            _visualDataSO = Resources.Load<VisualDataScriptableObject>("VisualDataScriptableObject");
        }
    }
}
