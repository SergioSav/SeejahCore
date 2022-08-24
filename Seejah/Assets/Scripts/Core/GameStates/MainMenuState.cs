using Assets.Scripts.Core.Framework;
using Assets.Scripts.Core.Models;
using UniRx;

namespace Assets.Scripts.Core.GameStates
{
    public class MainMenuState : DisposableContainer, IState
    {
        private GameModel _gameModel;

        public MainMenuState(GameModel gameModel)
        {
            _gameModel = gameModel;
        }

        public void OnEnter()
        {
            UnityEngine.Debug.Log("enter main menu");

            AddForDispose(_gameModel.NeedStartMatch.Subscribe(OnStartMatch));
        }

        private void OnStartMatch(bool needStart)
        {
            if (needStart)
            {
                _gameModel.ChangeGameStateTo(GameState.PrepareMatch);
            }
        }

        public void OnExit()
        {
            UnityEngine.Debug.Log("exit main menu");
            Dispose();
        }
    }
}
