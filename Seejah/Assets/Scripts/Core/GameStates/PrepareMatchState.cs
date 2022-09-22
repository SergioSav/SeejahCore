using Assets.Scripts.Core.Framework;
using Assets.Scripts.Core.Models;

namespace Assets.Scripts.Core.GameStates
{
    public class PrepareMatchState : DisposableContainer, IState
    {
        private GameModel _gameModel;

        public PrepareMatchState(GameModel gameModel)
        {
            _gameModel = gameModel;
        }

        public void OnEnter()
        {
            UnityEngine.Debug.Log("enter prepare match");
        }

        public void OnExit()
        {
            UnityEngine.Debug.Log("exit prepare match");
            Dispose();
        }
    }
}