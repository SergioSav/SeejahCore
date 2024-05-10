using Assets.Scripts.Core.Controllers;
using Assets.Scripts.Core.Data;
using Assets.Scripts.Core.HUD.Elements;
using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.Rules;
using Assets.Scripts.Core.Utils;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Assets.Scripts.Core.HUD
{
    public class SettingsMenuPresenter : BaseWindowPresenter
    {
        [SerializeField] private Button buttonSave;
        [SerializeField] private Toggle toggleSoundOn;
        [SerializeField] private Toggle toggleMusicOn;
        [SerializeField] private Toggle toggleRandomPlacement;
        [SerializeField] private BaseComboBoxPresenter comboBoxLanguage;
        [SerializeField] private TextMeshProUGUI textGameInfoLink;

        private GameRules _gameRules;
        private GameModel _gameModel;
        private IGameSettingsSetup _gameSettings;
        private AudioService _audioService;
        private LanguageService _languageService;
        private IPlatformService _platformService;
        private bool _isRandomPlacement;

        [Inject]
        public void Construct(GameRules gameRules,
                              GameModel gameModel,
                              IGameSettingsSetup gameSettings,
                              AudioService audioService,
                              LanguageService languageService,
                              IPlatformService platformService)
        {
            _gameRules = gameRules;
            _gameModel = gameModel;
            _gameSettings = gameSettings;
            _audioService = audioService;
            _languageService = languageService;
            _platformService = platformService;
        }

        private void OnStateChange(GameState state)
        {
            gameObject.SetActive(state == GameState.Settings);
            toggleMusicOn.isOn = _audioService.MusicOn.Value;
            toggleSoundOn.isOn = _audioService.SoundOn.Value;
            toggleRandomPlacement.isOn = _gameSettings.IsRandomPlacement;
        }

        private void Start()
        {
            Title = "Game Settings";
            CloseAction = OnClose;

            comboBoxLanguage.Setup(_languageService.GetInfoForSelection(), OnLanguageClick);
            comboBoxLanguage.SelectVariant(_languageService.CurrentLocale);


            AddForDispose(_gameModel.CurrentGameState.Subscribe(OnStateChange));

            AddForDispose(buttonSave
                .OnClickAsObservable()
                .Subscribe(_ =>
                {
                    _audioService.PlayUISound(SoundType.Click);
                    _gameSettings.SetRandomPlacementPhase(_isRandomPlacement);
                    _gameSettings.SaveChanges();
                }));

            AddForDispose(toggleMusicOn
                .OnValueChangedAsObservable()
                .Subscribe(isOn => OnMusicToggleSwitch(isOn)));

            AddForDispose(toggleSoundOn
                .OnValueChangedAsObservable()
                .Subscribe(isOn => OnSoundToggleSwitch(isOn)));

            AddForDispose(toggleRandomPlacement
                .OnValueChangedAsObservable()
                .Subscribe(isOn => OnRandomToggleSwitch(isOn)));

            AddForDispose(textGameInfoLink
                .OnPointerClickAsObservable()
                .Subscribe(_ => OnGameInfoLinkClick()));
        }

        private void OnGameInfoLinkClick()
        {
            Application.OpenURL(_gameRules.InfoLink);
        }

        private void OnMusicToggleSwitch(bool isOn)
        {
            _audioService.PlayUISound(SoundType.ToggleSwitch);
            _audioService.SwitchMusic(isOn);
        }

        private void OnSoundToggleSwitch(bool isOn)
        {
            _audioService.PlayUISound(SoundType.ToggleSwitch);
            _audioService.SwitchSound(isOn);
        }

        private void OnRandomToggleSwitch(bool isOn)
        {
            _audioService.PlayUISound(SoundType.ToggleSwitch);
            _isRandomPlacement = isOn;
        }

        private void OnLanguageClick(string languageId)
        {
            _audioService.PlayUISound(SoundType.Click);
            _languageService.SwitchLocaleTo(languageId);
        }

        private void OnClose()
        {
            _audioService.PlayUISound(SoundType.Click);
            _gameModel.CloseSettings();
            
            SendMetrics();
        }

        private void SendMetrics()
        {
            var metricsParam = new Dictionary<string, string>()
            {
                { MetricsConst.MusicSettings, _audioService.MusicOn.Value.ToString() },
                { MetricsConst.SoundSettings, _audioService.SoundOn.Value.ToString() },
                { MetricsConst.RandomPlaceSettings, _isRandomPlacement.ToString() },
                { MetricsConst.LanguageSettings, _languageService.CurrentLocale }
            };
            _platformService.SendMetric(MetricsConst.Settings, metricsParam);
        }
    }
}
