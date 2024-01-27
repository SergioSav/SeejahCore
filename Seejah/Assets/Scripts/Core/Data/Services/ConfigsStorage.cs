using Assets.Scripts.Core.Data.ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.Data.Services
{
    public class ConfigsStorage
    {
        public List<VisualData> VisualDataList => _visualDataSO.VisualDataList;
        public List<CustomizationData> CustomizationDataList=> _customizationDataSO.CustomizationDataList;
        public List<TutorialData> TutorialDataList => _tutorialDataSO.TutorialDataList;
        public List<SoundData> SoundDataList => _soundDataSO.SoundDataList;
        public GameRulesData GameRulesData => _gameRulesDataSO.rulesData;

        private GameRulesDataScriptableObject _gameRulesDataSO;
        private VisualDataScriptableObject _visualDataSO;
        private CustomizationDataScriptableObject _customizationDataSO;
        private TutorialDataScriptableObject _tutorialDataSO;
        private SoundDataScriptableObject _soundDataSO;

        public ConfigsStorage()
        {
            LoadConfigs();
        }

        private void LoadConfigs()
        {
            _gameRulesDataSO = Resources.Load<GameRulesDataScriptableObject>("GameRulesDataScriptableObject");
            _visualDataSO = Resources.Load<VisualDataScriptableObject>("VisualDataScriptableObject");
            _customizationDataSO = Resources.Load<CustomizationDataScriptableObject>(nameof(CustomizationDataScriptableObject));
            _tutorialDataSO = Resources.Load<TutorialDataScriptableObject>(nameof(TutorialDataScriptableObject));
            _soundDataSO = Resources.Load<SoundDataScriptableObject>(nameof(SoundDataScriptableObject));
        }
    }
}
