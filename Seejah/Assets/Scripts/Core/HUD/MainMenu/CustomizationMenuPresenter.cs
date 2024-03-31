using Assets.Scripts.Core.Controllers;
using Assets.Scripts.Core.HUD.Elements;
using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.SceneInstallers;
using Assets.Scripts.Core.Utils;
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
        private AudioService _audioService;

        [Inject]
        public void Construct(GameModel gameModel, CustomizationModel customizationModel, AudioService audioService)
        {
            _gameModel = gameModel;
            _customizationModel = customizationModel;
            _audioService = audioService;
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

            AddForDispose(customizeChipButton.OnPointerClickAsObservable().Subscribe(_ => OnChipClick()));
            AddForDispose(customizeChipColorButton.OnPointerClickAsObservable().Subscribe(_ => OnChipColorClick()));
            AddForDispose(customizeBoardButton.OnPointerClickAsObservable().Subscribe(_ => OnBoardClick()));
            AddForDispose(customizeFloorButton.OnPointerClickAsObservable().Subscribe(_ => OnFloorClick()));
        }

        private void OnChipClick()
        {
            _audioService.PlayUISound(SoundType.Click);
            _customizationModel.StartChipCustomization();
        }
        private void OnChipColorClick()
        {
            _audioService.PlayUISound(SoundType.Click);
            _customizationModel.StartChipColorCustomization();
        }
        private void OnBoardClick()
        {
            _audioService.PlayUISound(SoundType.Click);
            _customizationModel.StartBoardCustomization();
        }
        private void OnFloorClick()
        {
            _audioService.PlayUISound(SoundType.Click);
            _customizationModel.StartFloorCustomization();
        }

        private void OnMenuClose()
        {
            _audioService.PlayUISound(SoundType.Click);
            _gameModel.EndCustomization();
        }
    }
}
