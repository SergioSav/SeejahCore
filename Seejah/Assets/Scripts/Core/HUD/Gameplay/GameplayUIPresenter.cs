using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.Presenters;
using TMPro;
using UniRx;
using UnityEngine;
using VContainer;

namespace Assets.Scripts.Core.HUD
{
    public class GameplayUIPresenter : MonoBehPresenter
    {
        [SerializeField] private TextMeshProUGUI textCurrentTeam;

        private MatchModel _matchModel;

        [Inject]
        public void Construct(MatchModel matchModel)
        {
            _matchModel = matchModel;
        }

        private void Start()
        {
            AddForDispose(_matchModel.WaitNextTurn.Subscribe(_ => OnWaitNextTurn()));
        }

        private void OnWaitNextTurn()
        {
            textCurrentTeam.text = _matchModel.ActivePlayer.TeamType.ToString();
        }
    }
}
