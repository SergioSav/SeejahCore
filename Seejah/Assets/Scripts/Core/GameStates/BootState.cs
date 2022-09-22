using Assets.Scripts.Core.Framework;
using Assets.Scripts.Core.Models;

namespace Assets.Scripts.Core.GameStates
{
    public class BootState : DisposableContainer, IState
    {
        private readonly GameModel _gameModel;

        public BootState(GameModel gameModel)
        {
            _gameModel = gameModel;
        }

        public void OnEnter()
        {
            UnityEngine.Debug.Log("enter boot");
            // init smthng
            _gameModel.ChangeGameStateTo(GameState.Loading);
        }

        public void OnExit()
        {
            UnityEngine.Debug.Log("exit boot");
            Dispose();
        }
    }
}
