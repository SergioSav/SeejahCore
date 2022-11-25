using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.Presenters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using UniRx;

namespace Assets.Scripts.Core.HUD
{
    public class MainMenuPresenter : MonoBehPresenter
    {
        [SerializeField] private TextMeshProUGUI textTitle;
        [SerializeField] private Button buttonStart;
        [SerializeField] private Button buttonStartWithRandomPlacement;
        [SerializeField] private Button buttonStartWithUltimateAI;
        [SerializeField] private Button buttonCustomize;

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

            AddForDispose(buttonStartWithUltimateAI
                .OnClickAsObservable()
                .Subscribe(_ => _gameModel.StartMatchWithUltimateAI()));

            AddForDispose(buttonStartWithRandomPlacement
                .OnClickAsObservable()
                .Subscribe(_ => _gameModel.StartMatchWithRandomPlacement()));

            AddForDispose(buttonCustomize
                .OnClickAsObservable()
                .Subscribe(_ => _gameModel.StartCusomization()));
        }
    }
}
