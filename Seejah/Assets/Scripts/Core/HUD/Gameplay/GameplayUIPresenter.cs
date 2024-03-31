using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.Presenters;
using Assets.Scripts.Core.Utils;
using DG.Tweening;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;
using VContainer;

namespace Assets.Scripts.Core.HUD
{
    public class GameplayUIPresenter : MonoBehPresenter
    {
        [SerializeField] private TextMeshProUGUI textCurrentTeam;
        [SerializeField] private TextMeshProUGUI textInfoBanner;
        [SerializeField] private Button buttonHelp;
        [SerializeField] private Toggle toggleMusic;
        [SerializeField] private Toggle toggleSound;
        [SerializeField] private Transform infoBanner;
        [SerializeField] private CanvasGroup infoBannerCanvas;
        [SerializeField] private LocalizedString bannerMidPlayerTurn;
        [SerializeField] private LocalizedString bannerMidEndGame;
        [SerializeField] private LocalizedString bannerMidPlacementStart;
        [SerializeField] private LocalizedString bannerMidPlacementDone;  
        [SerializeField] private LocalizedString bannerTopPlayerTurn;
        [SerializeField] private LocalizedString bannerTopOpponentTurn;
        [SerializeField] private LocalizedString bannerTopGameStarted;

        private MatchModel _matchModel;
        private GameplayUIModel _gameplayUIModel;
        private AudioService _audioService;
        private Sequence _bannerSequence;

        private string _bannerMidPlayerTurnLocalized;
        private string _bannerMidEndGameLocalized;
        private string _bannerMidPlacementDoneLocalized;
        private string _bannerMidPlacementStartLocalized;
        private string _bannerTopPlayerTurnLocalized;
        private string _bannerTopOpponentTurnLocalized;
        private string _bannerTopGameStartedLocalized;

        [Inject]
        public void Construct(MatchModel matchModel, GameplayUIModel gameplayUIModel, AudioService audioService)
        {
            _matchModel = matchModel;
            _gameplayUIModel = gameplayUIModel;
            _audioService = audioService;
        }

        private void Start()
        {
            _bannerMidPlayerTurnLocalized = bannerMidPlayerTurn.GetLocalizedString();
            _bannerMidEndGameLocalized = bannerMidEndGame.GetLocalizedString();
            _bannerMidPlacementStartLocalized = bannerMidPlacementStart.GetLocalizedString();
            _bannerMidPlacementDoneLocalized = bannerMidPlacementDone.GetLocalizedString();
            _bannerTopPlayerTurnLocalized = bannerTopPlayerTurn.GetLocalizedString();
            _bannerTopOpponentTurnLocalized = bannerTopOpponentTurn.GetLocalizedString();
            _bannerTopGameStartedLocalized = bannerTopGameStarted.GetLocalizedString();

            textCurrentTeam.text = _bannerTopGameStartedLocalized;

            AddForDispose(_matchModel.CurrentState.Subscribe(OnMatchStateChange));
            AddForDispose(_matchModel.WaitNextTurn.Subscribe(_ => OnWaitNextTurn()));
            AddForDispose(buttonHelp.OnPointerClickAsObservable().Subscribe(_ => OnHelpClick()));
            AddForDispose(toggleMusic
                .OnValueChangedAsObservable()
                .Subscribe(isOff => _audioService.SwitchMusic(!isOff)));
            AddForDispose(toggleSound
                .OnValueChangedAsObservable()
                .Subscribe(isOff => _audioService.SwitchSound(!isOff)));

            InitBannerAnimator();

            toggleMusic.isOn = !_audioService.MusicOn.Value;
            toggleSound.isOn = !_audioService.SoundOn.Value;
        }

        private void OnHelpClick()
        {
            _audioService.PlayUISound(SoundType.Click);
            _gameplayUIModel.ShowTutorialWindow();
        }

        private void InitBannerAnimator()
        {
            infoBanner.gameObject.SetActive(false);
            infoBanner.localPosition = Vector3.up * -100;
            infoBannerCanvas.alpha = 0.2f;

            _bannerSequence = DOTween.Sequence();
            _bannerSequence.SetLoops(2, LoopType.Yoyo);
            _bannerSequence.SetAutoKill(false);
            _bannerSequence.Append(infoBanner.DOLocalMoveY(0, 0.5f));
            _bannerSequence.Join(infoBannerCanvas.DOFade(1, 0.6f));
            _bannerSequence.AppendInterval(1);
            _bannerSequence.onComplete += () => infoBanner.gameObject.SetActive(false);
            _bannerSequence.Pause();
        }

        private void OnMatchStateChange(MatchStateType state)
        {
            switch (state)
            {
                case MatchStateType.None:
                    break;
                case MatchStateType.Loading:
                    break;
                case MatchStateType.Ready:
                    if (_matchModel.TryShowTutorial())
                        _gameplayUIModel.ShowTutorialWindow();
                    else
                        _gameplayUIModel.ReturnNormalState();
                    break;
                case MatchStateType.PhasePlacement:
                    ShowBanner(_bannerMidPlacementStartLocalized);
                    break;
                case MatchStateType.PlacementDone:
                    UpdateTopBanner();
                    ShowBanner(_bannerMidPlacementDoneLocalized);
                    break;
                case MatchStateType.PhaseBattle:
                    break;
                case MatchStateType.BattleEnd:
                    ShowBanner(_bannerMidEndGameLocalized);
                    break;
            }
        }

        private void UpdateTopBanner()
        {
            textCurrentTeam.text = _matchModel.IsUserTurn ? _bannerTopPlayerTurnLocalized : _bannerTopOpponentTurnLocalized;
        }

        private void ShowBanner(string info)
        {
            infoBanner.gameObject.SetActive(true);
            textInfoBanner.text = info;
            _bannerSequence.Restart();
        }

        private void OnWaitNextTurn()
        {
            if (_matchModel.IsUserTurn)
                ShowBanner(_bannerMidPlayerTurnLocalized);

            UpdateTopBanner();
        }

        private void OnDestroy()
        {
            _bannerSequence.Kill();
        }
    }
}
