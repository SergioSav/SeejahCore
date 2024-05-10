using Assets.Scripts.Core.Data;
using Assets.Scripts.Core.Data.Services;
using Assets.Scripts.Core.HUD.Elements;
using Assets.Scripts.Core.HUD.Gameplay.Tutorial;
using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.Utils;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using VContainer;

namespace Assets.Scripts.Core.HUD
{
    public class TutorialWindowPresenter : BaseWindowPresenter
    {
        [SerializeField] private PageInfoDotPresenter infoDotPrototype;
        [SerializeField] private TutorialWindowVerticalContent verticalContent;
        [SerializeField] private TutorialWindowHorizontalContent horizontalContent;

        private ITutorialWindowContent _currentContent;

        private Func<PageInfoDotPresenter, Transform, PageInfoDotPresenter> _infoDotFactory;
        private ISpriteSupplier _spriteSupplier;
        private GameplayUIModel _gameplayUIModel;
        private List<TutorialData> _tutorialInfoList;
        private int _currentIndex;
        private List<PageInfoDotPresenter> _infoDots;
        private AudioService _audioService;
        private IPlatformService _platformService;
        private bool _isContentHorizontal;

        [Inject]
        public void Construct(ConfigsStorage configStorage, GameplayUIModel gameplayUIModel,
                              Func<PageInfoDotPresenter, Transform, PageInfoDotPresenter> infoDotFactory,
                              ISpriteSupplier spriteSupplier, AudioService audioService, IPlatformService platformService)
        {
            _infoDotFactory = infoDotFactory;
            _spriteSupplier = spriteSupplier;
            _gameplayUIModel = gameplayUIModel;
            _audioService = audioService;
            _platformService = platformService;

            _tutorialInfoList = configStorage.TutorialDataList;
            _currentIndex = 0;
            _infoDots = new List<PageInfoDotPresenter>();
        }

        private void OnMatchStateChange(GameplayUIState state)
        {
            var needShow = state == GameplayUIState.TutorialWindow;
            if (needShow)
                _platformService.SendMetric(MetricsConst.TutorialShow);
            gameObject.SetActive(needShow);
        }

        private void Update()
        {
            if (IsLandscape)
            {
                if (!_isContentHorizontal)
                    SwitchToHorizontal();
            }
            else
            {
                if (_isContentHorizontal)
                    SwitchToVertical();
            }
        }

        private void SwitchToVertical()
        {
            _isContentHorizontal = false;
            _currentContent?.UpdateVisibility(false);
            _currentContent = verticalContent;
            _currentContent.UpdateVisibility(true);
            UpdateContent();
        }

        private void SwitchToHorizontal()
        {
            _isContentHorizontal = true;
            _currentContent?.UpdateVisibility(false);
            _currentContent = horizontalContent;
            _currentContent.UpdateVisibility(true);
            UpdateContent();
        }

        private void UpdateContent()
        {
            ClearInfoDots();
            CreateInfoDots();
            HandleInfoState();
        }

        private void Start()
        {
            verticalContent.Setup(OnRightClick, OnLeftClick, OnNextClick, CloseWindow);
            verticalContent.UpdateVisibility(false);
            horizontalContent.Setup(OnRightClick, OnLeftClick, OnNextClick, CloseWindow);
            horizontalContent.UpdateVisibility(false);

            SwitchToHorizontal();

            AddForDispose(_gameplayUIModel.CurrentState.Subscribe(OnMatchStateChange));
        }

        private void OnNextClick()
        {
            if (_currentIndex >= _tutorialInfoList.Count - 1)
            {
                CloseWindow();
            }
            else
            {
                _audioService.PlayUISound(SoundType.Click);
                OnRightClick();
            }
        }

        private void OnRightClick()
        {
            _audioService.PlayUISound(SoundType.Click);
            _currentIndex = Math.Min(_currentIndex + 1, _tutorialInfoList.Count - 1);
            HandleInfoState();
        }

        private void OnLeftClick()
        {
            _audioService.PlayUISound(SoundType.Click);
            _currentIndex = Math.Max(_currentIndex - 1, 0);
            HandleInfoState();
        }

        private void CloseWindow()
        {
            _audioService.PlayUISound(SoundType.Click);
            _gameplayUIModel.ReturnNormalState();
        }

        private void HandleInfoState()
        {
            _currentContent.LeftArrow.enabled = _currentIndex > 0;
            _currentContent.RightArrow.enabled = _currentIndex < _tutorialInfoList.Count - 1;
            _currentContent.NextButton.enabled = _currentIndex < _tutorialInfoList.Count - 1;

            for (int i = 0; i < _infoDots.Count; i++)
                _infoDots[i].SwitchActive(i == _currentIndex);

            _currentContent.SetText(_tutorialInfoList[_currentIndex].Message.GetLocalizedString());
            _currentContent.SetImage(_spriteSupplier.GetSprite(_tutorialInfoList[_currentIndex].ImageId));
        }

        private void CreateInfoDots()
        {
            for (int i = 0; i < _tutorialInfoList.Count; i++)
            {
                var item = _infoDotFactory.Invoke(infoDotPrototype, _currentContent.DotsPlace);
                _infoDots.Add(item);
            }
        }

        private void ClearInfoDots()
        {
            foreach (var dot in _infoDots)
                Destroy(dot.gameObject);
            _infoDots.Clear();
        }
    }
}
