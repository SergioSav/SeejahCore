using Assets.Scripts.Core.Controllers;
using Assets.Scripts.Core.HUD.Elements;
using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.Rules;
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
        [SerializeField] private DropItemPresenter dropItem;

        private GameModel _gameModel;
        private GameRules _gameRules;
        private UserModel _userModel;
        private bool _isWin;

        [Inject]
        public void Construct(GameModel gameModel, GameRules gameRules, UserModel userModel)
        {
            _gameModel = gameModel;
            _gameRules = gameRules;
            _userModel = userModel;
        }

        private void OnStateChange(GameState state)
        {
            gameObject.SetActive(state == GameState.Reward);
            if (state == GameState.Reward)
            {
                _isWin = _gameModel.LastWinner.TeamType == _userModel.TeamType;
                textGameResult.text = "You " + (_isWin ? "WIN!" : "lose...");
                textButtonApply.text = _isWin ? "Confirm" : "Return to menu";
                Title = _isWin ? "Congratulation!" : "Match is over";
                dropItem.Setup(_gameRules.WinDrop);
                dropItem.gameObject.SetActive(_isWin);
            }
        }

        private void Start()
        {
            CloseAction = OnApplyClick;

            AddForDispose(_gameModel.CurrentGameState.Subscribe(OnStateChange));
            AddForDispose(buttonApply.OnClickAsObservable()
                .Subscribe(_ => OnApplyClick()));
        }

        private void OnApplyClick()
        {
            if (_isWin)
                _userModel.ApplyDrop(_gameRules.WinDrop);
            _gameModel.EndMatch();
        }
    }
}
