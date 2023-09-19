using Assets.Scripts.Core.Data;
using Assets.Scripts.Core.Data.Services;
using Assets.Scripts.Core.HUD.Elements;
using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.SceneInstallers;
using System;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Assets.Scripts.Core.HUD
{
    public class TutorialWindowPresenter : BaseWindowPresenter
    {
        [SerializeField] private Image imageTutorial;
        [SerializeField] private TextMeshProUGUI textTutorial;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button leftArrowButton;
        [SerializeField] private Button rightArrowButton;
        [SerializeField] private PageInfoDotPresenter infoDotPrototype;
        [SerializeField] private Transform infoDotPlace;

        private UserModel _userModel;
        private CustomizationModel _customizationModel;
        private ConfigsStorage _configStorage;

        private Func<PageInfoDotPresenter, Transform, PageInfoDotPresenter> _infoDotFactory;
        private ISpriteSupplier _spriteSupplier;
        private MatchModel _matchModel;
        private GameplayUIModel _gameplayUIModel;
        private List<TutorialData> _tutorialInfoList;
        private int _currentIndex;
        private List<PageInfoDotPresenter> _infoDots;

        [Inject]
        public void Construct(UserModel userModel, ConfigsStorage configStorage, 
                MatchModel matchModel, GameplayUIModel gameplayUIModel,
                Func<PageInfoDotPresenter, Transform, PageInfoDotPresenter> infoDotFactory, 
                ISpriteSupplier spriteSupplier)
        {
            _userModel = userModel;
            _configStorage = configStorage;
            _infoDotFactory = infoDotFactory;
            _spriteSupplier = spriteSupplier;
            _matchModel = matchModel;
            _gameplayUIModel = gameplayUIModel;

            _tutorialInfoList = configStorage.TutorialDataList;
            _currentIndex = 0;
            _infoDots = new List<PageInfoDotPresenter>();
        }

        private void OnMatchStateChange(GameplayUIState state)
        {
            gameObject.SetActive(state == GameplayUIState.TutorialWindow);
        }

        private void Start()
        {
            Title = "How to play";
            CloseAction = CloseWindow;

            CreateInfoDots();
            HandleInfoState();

            AddForDispose(_gameplayUIModel.CurrentState.Subscribe(OnMatchStateChange));
            AddForDispose(nextButton.OnPointerClickAsObservable().Subscribe(_ => OnNextClick()));
            AddForDispose(leftArrowButton.OnPointerClickAsObservable().Subscribe(_ => OnLeftClick()));
            AddForDispose(rightArrowButton.OnPointerClickAsObservable().Subscribe(_ => OnRightClick()));
        }

        private void OnNextClick()
        {
            if (_currentIndex >= _tutorialInfoList.Count - 1)
                CloseWindow();
            else
                OnRightClick();
        }

        private void OnRightClick()
        {
            _currentIndex = Math.Min(_currentIndex + 1, _tutorialInfoList.Count - 1);
            HandleInfoState();
        }

        private void OnLeftClick()
        {
            _currentIndex = Math.Max(_currentIndex - 1, 0);
            HandleInfoState();
        }

        private void CloseWindow()
        {
            Debug.Log("close");
            _gameplayUIModel.ReturnNormalState();
        }

        private void HandleInfoState()
        {
            leftArrowButton.enabled = _currentIndex > 0;
            rightArrowButton.enabled = _currentIndex < _tutorialInfoList.Count - 1;
            nextButton.enabled = _currentIndex < _tutorialInfoList.Count - 1;

            for (int i = 0; i < _infoDots.Count; i++)
                _infoDots[i].SwitchActive(i == _currentIndex);

            textTutorial.text = _tutorialInfoList[_currentIndex].Message;
            imageTutorial.sprite = _spriteSupplier.GetSprite(_tutorialInfoList[_currentIndex].ImageId);
        }

        private void CreateInfoDots()
        {
            for (int i = 0; i < _tutorialInfoList.Count; i++)
            {
                var item = _infoDotFactory.Invoke(infoDotPrototype, infoDotPlace);
                _infoDots.Add(item);
            }
        }
    }
}
