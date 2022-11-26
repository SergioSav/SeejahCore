using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.Presenters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using UniRx;
using Assets.Scripts.Core.Data;

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
        private IGameSettingsSetup _gameSettings;

        [Inject]
        public void Construct(GameModel gameModel, IGameSettingsSetup gameSettings)
        {
            _gameModel = gameModel;
            _gameSettings = gameSettings;
        }

        private void Start()
        {
            AddForDispose(buttonStart
                .OnClickAsObservable()
                .Subscribe(_ => _gameModel.StartMatch()));

            AddForDispose(buttonStartWithUltimateAI
                .OnClickAsObservable()
                .Subscribe(_ =>
                {
                    _gameSettings.SetUsingUltimateAI(true);
                    _gameModel.StartMatch();
                }));

            AddForDispose(buttonStartWithRandomPlacement
                .OnClickAsObservable()
                .Subscribe(_ =>
                {
                    _gameSettings.SetRandomPlacementPhase(true);
                    _gameModel.StartMatch();
                }));

            AddForDispose(buttonCustomize
                .OnClickAsObservable()
                .Subscribe(_ => _gameModel.StartCusomization()));
        }
    }
}
