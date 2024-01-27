using Assets.Scripts.Core.Controllers;
using Assets.Scripts.Core.Data;
using Assets.Scripts.Core.HUD.Elements;
using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.Utils.AudioService;
using System;
using UniRx;
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

        private GameModel _gameModel;
        private IGameSettingsSetup _gameSettings;
        private AudioService _audioService;

        private bool _isRandomPlacement;

        [Inject]
        public void Construct(GameModel gameModel, IGameSettingsSetup gameSettings, AudioService audioService)
        {
            _gameModel = gameModel;
            _gameSettings = gameSettings;
            _audioService = audioService;
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

            AddForDispose(_gameModel.CurrentGameState.Subscribe(OnStateChange));

            AddForDispose(buttonSave
                .OnClickAsObservable()
                .Subscribe(_ =>
                {
                    _gameSettings.SetRandomPlacementPhase(_isRandomPlacement);
                    _gameSettings.SaveChanges();
                }));

            AddForDispose(toggleMusicOn
                .OnValueChangedAsObservable()
                .Subscribe(isOn => _audioService.SwitchMusic(isOn)));

            AddForDispose(toggleSoundOn
                .OnValueChangedAsObservable()
                .Subscribe(isOn => _audioService.SwitchSound(isOn)));

            AddForDispose(toggleRandomPlacement
                .OnValueChangedAsObservable()
                .Subscribe(isOn => _isRandomPlacement = isOn));
        }

        private void OnClose()
        {
            _audioService.PlayUISound(SoundType.Click);
            _gameModel.CloseSettings();
        }
    }
}
