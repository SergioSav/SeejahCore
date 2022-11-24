using Assets.Scripts.Core.Controllers;
using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.Presenters;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Assets.Scripts.Core.HUD
{
    public class EndGameWindowPresenter : MonoBehPresenter
    {
        [SerializeField] private TextMeshProUGUI textTitle;
        [SerializeField] private TextMeshProUGUI textGameResult;
        [SerializeField] private Button buttonClose;

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
                textGameResult.text = "You " + (_gameModel.LastWinner.TeamType == _userModel.TeamType ? "WIN!" : "lose...");
            }
        }

        private void Start()
        {
            AddForDispose(_gameModel.CurrentGameState.Subscribe(OnStateChange));
            AddForDispose(buttonClose
                .OnClickAsObservable()
                .Subscribe(_ => _gameModel.EndMatch()));
        }
    }
}
