using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.Presenters;
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

        [Inject]
        public void Construct(GameModel gameModel)
        {
            _gameModel = gameModel;
        }

        private void Start()
        {
            AddForDispose(buttonStart
                .OnClickAsObservable()
                .Subscribe(_ => _gameModel.StartMatch()));

            AddForDispose(buttonSettings
                .OnClickAsObservable()
                .Subscribe(_ => _gameModel.OpenSettings()));

            AddForDispose(buttonCustomize
                .OnClickAsObservable()
                .Subscribe(_ => _gameModel.StartCusomization()));
        }
    }
}
