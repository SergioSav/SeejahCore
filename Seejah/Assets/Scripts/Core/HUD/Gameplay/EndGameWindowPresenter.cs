using Assets.Scripts.Core.Controllers;
using Assets.Scripts.Core.HUD.Elements;
using Assets.Scripts.Core.Models;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Assets.Scripts.Core.HUD
{
    public class EndGameWindowPresenter : BaseWindowPresenter
    {
        [SerializeField] private TextMeshProUGUI textGameResult;
        [SerializeField] private TextMeshProUGUI textButtonApply;
        [SerializeField] private Button buttonApply;

        private GameModel _gameModel;
        private UserModel _userModel;

        [Inject]
        public void Construct(GameModel gameModel, UserModel userModel)
        {
            _gameModel = gameModel;
            _userModel = userModel;
        }

        private void OnStateChange(GameState state)
        {
            gameObject.SetActive(state == GameState.Reward);
            if (state == GameState.Reward)
            {
                var isWin = _gameModel.LastWinner.TeamType == _userModel.TeamType;
                textGameResult.text = "You " + (isWin ? "WIN!" : "lose...");
                textButtonApply.text = isWin ? "Confirm" : "Return to menu";
                Title = isWin ? "Congratulation!" : "Match is over";
            }
        }

        private void Start()
        {
            CloseAction = _gameModel.EndMatch;

            AddForDispose(_gameModel.CurrentGameState.Subscribe(OnStateChange));
            AddForDispose(buttonApply.OnClickAsObservable()
                .Subscribe(_ => _gameModel.EndMatch()));
        }
    }
}
