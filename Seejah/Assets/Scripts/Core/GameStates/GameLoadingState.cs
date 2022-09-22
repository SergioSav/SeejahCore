using Assets.Scripts.Core.Framework;
using Assets.Scripts.Core.Models;

namespace Assets.Scripts.Core.GameStates
{
    public class GameLoadingState : DisposableContainer, IState
    {
        private readonly GameModel _gameModel;

        public GameLoadingState(GameModel gameModel)
        {
            _gameModel = gameModel;
        }

        public void OnEnter()
        {
            UnityEngine.Debug.Log("enter loading");
            _gameModel.ChangeGameStateTo(GameState.MainMenu);
        }

        public void OnExit()
        {
            UnityEngine.Debug.Log("exit loading");
        }
    }
}
