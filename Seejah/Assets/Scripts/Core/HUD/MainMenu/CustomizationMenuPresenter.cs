using Assets.Scripts.Core.Controllers;
using Assets.Scripts.Core.HUD.Elements;
using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.SceneInstallers;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Assets.Scripts.Core.HUD
{
    public class CustomizationMenuPresenter : BaseWindowPresenter
    {
        [SerializeField] private Button customizeChipButton;
        [SerializeField] private Button customizeChipColorButton;
        [SerializeField] private Button customizeBoardButton;
        [SerializeField] private Button customizeFloorButton;

        private GameModel _gameModel;
        private CustomizationModel _customizationModel;

        [Inject]
        public void Construct(GameModel gameModel, CustomizationModel customizationModel)
        {
            _gameModel = gameModel;
            _customizationModel = customizationModel;
        }

        private void OnStateChange(GameState state)
        {
            gameObject.SetActive(state == GameState.Customization);
        }

        private void Start()
        {
            Title = "Customization";
            CloseAction = OnMenuClose;

            AddForDispose(_gameModel.CurrentGameState.Subscribe(OnStateChange));

            AddForDispose(customizeChipButton.OnPointerClickAsObservable().Subscribe(_ => _customizationModel.StartChipCustomization()));
            AddForDispose(customizeChipColorButton.OnPointerClickAsObservable().Subscribe(_ => _customizationModel.StartChipColorCustomization()));
            AddForDispose(customizeBoardButton.OnPointerClickAsObservable().Subscribe(_ => _customizationModel.StartBoardCustomization()));
            AddForDispose(customizeFloorButton.OnPointerClickAsObservable().Subscribe(_ => _customizationModel.StartFloorCustomization()));
        }

        private void OnMenuClose()
        {
            _gameModel.EndCustomization();
        }
    }
}
