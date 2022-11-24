using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.Presenters;
using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;
using VContainer;

namespace Assets.Scripts.Core.HUD
{
    public class GameplayUIPresenter : MonoBehPresenter
    {
        [SerializeField] private TextMeshProUGUI textCurrentTeam;
        [SerializeField] private TextMeshProUGUI textInfoBanner;
        [SerializeField] private Transform infoBanner;
        [SerializeField] private CanvasGroup infoBannerCanvas;

        private MatchModel _matchModel;
        private Sequence _bannerSequence;

        [Inject]
        public void Construct(MatchModel matchModel)
        {
            _matchModel = matchModel;
        }

        private void Start()
        {
            AddForDispose(_matchModel.CurrentState.Subscribe(OnMatchStateChange));
            AddForDispose(_matchModel.WaitNextTurn.Subscribe(_ => OnWaitNextTurn()));
            InitBannerAnimator();
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
                    ShowBanner("Match started! Place chips");
                    break;
                case MatchStateType.PhasePlacement:
                    break;
                case MatchStateType.PlacementDone:
                    ShowBanner("Placement done! Let's play!");
                    break;
                case MatchStateType.PhaseBattle:
                    break;
                case MatchStateType.BattleEnd:
                    ShowBanner("Match finished");
                    break;
            }
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
                ShowBanner($"Your turn!");
        }

        private void OnDestroy()
        {
            _bannerSequence.Kill();
        }
    }
}
