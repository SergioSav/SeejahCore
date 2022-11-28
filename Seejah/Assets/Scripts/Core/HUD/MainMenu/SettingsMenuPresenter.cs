using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.Presenters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using UniRx;
using Assets.Scripts.Core.Data;
using Assets.Scripts.Core.Controllers;

namespace Assets.Scripts.Core.HUD
{
    public class SettingsMenuPresenter : MonoBehPresenter
    {
        [SerializeField] private TextMeshProUGUI textTitle;
        [SerializeField] private Button buttonClose;
        [SerializeField] private Button buttonSave;
        [SerializeField] private Toggle toggleUltimateAI;
        [SerializeField] private Toggle toggleRandomPlacement;

        private GameModel _gameModel;
        private IGameSettingsSetup _gameSettings;

        private bool _needUseUltimateAI = false;
        private bool _isRandomPlacement = false;

        [Inject]
        public void Construct(GameModel gameModel, IGameSettingsSetup gameSettings)
        {
            _gameModel = gameModel;
            _gameSettings = gameSettings;
        }

        private void OnStateChange(GameState state)
        {
            gameObject.SetActive(state == GameState.Settings);
            toggleUltimateAI.isOn = _gameSettings.NeedUseUltimateAI;
            toggleRandomPlacement.isOn = _gameSettings.IsRandomPlacementPhase;
        }

        private void Start()
        {
            AddForDispose(_gameModel.CurrentGameState.Subscribe(OnStateChange));
            AddForDispose(buttonClose
                .OnClickAsObservable()
                .Subscribe(_ => _gameModel.CloseSettings()));

            AddForDispose(buttonSave
                .OnClickAsObservable()
                .Subscribe(_ =>
                {
                    _gameSettings.SetUsingUltimateAI(_needUseUltimateAI);
                    _gameSettings.SetRandomPlacementPhase(_isRandomPlacement);
                    _gameSettings.SaveChanges();
                }));
            
            AddForDispose(toggleUltimateAI
                .OnValueChangedAsObservable()
                .Subscribe(isOn => _needUseUltimateAI = isOn));
            
            AddForDispose(toggleRandomPlacement
                .OnValueChangedAsObservable()
                .Subscribe(isOn => _isRandomPlacement = isOn));
        }
    }
}
