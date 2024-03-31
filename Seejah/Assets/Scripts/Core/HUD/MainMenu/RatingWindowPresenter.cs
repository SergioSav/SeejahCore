using Assets.Scripts.Core.Controllers;
using Assets.Scripts.Core.HUD.Elements;
using Assets.Scripts.Core.Models;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Assets.Scripts.Core.HUD
{
    public class RatingWindowPresenter : BaseWindowPresenter
    {
        [SerializeField] private Button buttonConfirm;

        private GameModel _gameModel;

        [Inject]
        public void Construct(GameModel gameModel)
        {
            _gameModel = gameModel;
        }

        private void OnStateChange(GameState state)
        {
            gameObject.SetActive(state == GameState.Rating);
        }

        private void Start()
        {
            CloseAction = CloseWindow;

            AddForDispose(_gameModel.CurrentGameState.Subscribe(OnStateChange));

            AddForDispose(buttonConfirm.OnClickAsObservable()
                .Subscribe(_ => CloseWindow()));
        }

        private void CloseWindow()
        {
            _gameModel.CloseRating();
        }
    }
}
