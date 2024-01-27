using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.Presenters;
using Assets.Scripts.Core.Utils.AudioService;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Assets.Scripts.Core.HUD
{
    public class MainMenuPresenter : MonoBehPresenter
    {
        [SerializeField] private TextMeshProUGUI textTitle;
        [SerializeField] private Button buttonStart;
        [SerializeField] private Button buttonCustomize;
        [SerializeField] private Button buttonSettings;

        private GameModel _gameModel;
        private AudioService _audioService;

        [Inject]
        public void Construct(GameModel gameModel, AudioService audioService)
        {
            _gameModel = gameModel;
            _audioService = audioService;
        }

        private void Start()
        {
            AddForDispose(buttonStart
                .OnClickAsObservable()
                .Subscribe(_ => _gameModel.StartPrepareMatch()));

            AddForDispose(buttonSettings
                .OnClickAsObservable()
                .Subscribe(_ => _gameModel.OpenSettings()));

            AddForDispose(buttonCustomize
                .OnClickAsObservable()
                .Subscribe(_ => _gameModel.StartCusomization()));

            _audioService.PlayMusic();
        }
    }
}
