using Assets.Scripts.Core.Controllers;
using Assets.Scripts.Core.Data;
using Assets.Scripts.Core.HUD.Elements;
using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.Rules;
using Assets.Scripts.Core.Utils;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;
using VContainer;

namespace Assets.Scripts.Core.HUD
{
    public class EndGameWindowPresenter : BaseWindowPresenter
    {
        [SerializeField] private TextMeshProUGUI textGameResult;
        [SerializeField] private TextMeshProUGUI textButtonApply;
        [SerializeField] private TextMeshProUGUI textButtonRewardAds;
        [SerializeField] private Button buttonApply;
        [SerializeField] private Button buttonApplyWithAds;
        [SerializeField] private DropItemPresenter dropItem;
        [SerializeField] private RatingItemPresenter ratingItem;
        [SerializeField] private LocalizedString winTitle;
        [SerializeField] private LocalizedString winDescription;
        [SerializeField] private LocalizedString winButton;
        [SerializeField] private LocalizedString loseTitle;
        [SerializeField] private LocalizedString loseDescription;
        [SerializeField] private LocalizedString loseButton;

        private GameModel _gameModel;
        private GameRules _gameRules;
        private UserModel _userModel;
        private PlatformService _platformService;
        private bool _isWin;

        [Inject]
        public void Construct(GameModel gameModel, GameRules gameRules, UserModel userModel, PlatformService platformService)
        {
            _gameModel = gameModel;
            _gameRules = gameRules;
            _userModel = userModel;
            _platformService = platformService;
        }

        public override void Dispose()
        {
            base.Dispose();
            _platformService.RewardAdsSeen -= OnRewardAdsSeen;
        }

        private void OnStateChange(GameState state)
        {
            gameObject.SetActive(state == GameState.Reward);
            if (state == GameState.Reward)
            {
                _isWin = _gameModel.LastWinner.TeamType == _userModel.TeamType;
                textGameResult.text = _isWin ? winDescription.GetLocalizedString() : loseDescription.GetLocalizedString();
                textButtonApply.text = _isWin ? winButton.GetLocalizedString() : loseButton.GetLocalizedString();
                Title = _isWin ? winTitle.GetLocalizedString() : loseTitle.GetLocalizedString();
                dropItem.Setup(_gameRules.WinDrop);
                dropItem.gameObject.SetActive(_isWin);
                buttonApplyWithAds.gameObject.SetActive(_isWin);
                textButtonRewardAds.text = $"+{_gameRules.AdsReward.Value}";
                ratingItem.Setup(_isWin ? _gameRules.WinRatingChange : _gameRules.LoseRatingChange);
            }
        }

        private void Start()
        {
            CloseAction = OnApplyClick;

            AddForDispose(_gameModel.CurrentGameState.Subscribe(OnStateChange));
            AddForDispose(buttonApply.OnClickAsObservable()
                .Subscribe(_ => OnApplyClick()));
            AddForDispose(buttonApplyWithAds.OnClickAsObservable()
                .Subscribe(_ => OnApplyWithAdsClick()));
            _platformService.RewardAdsSeen += OnRewardAdsSeen;
        }

        private void OnApplyWithAdsClick()
        {
            _platformService.ShowRewardsAds();
        }

        private void OnRewardAdsSeen(DropData data)
        {
            ChangeRating();
            _userModel.ApplyDrop(_gameRules.WinDrop.Merge(_gameRules.AdsReward));
            _gameModel.EndMatch();
        }

        private void OnApplyClick()
        {
            ChangeRating();
            if (_isWin)
                _userModel.ApplyDrop(_gameRules.WinDrop);
            _gameModel.EndMatch();
        }

        private void ChangeRating()
        {
            _userModel.ChangeRatingScore(_isWin ? _gameRules.WinRatingChange : _gameRules.LoseRatingChange);
            _platformService.ApplyRatingScore(_userModel.RatingScore);
        }
    }
}
