using Assets.Scripts.Core.Framework;
using Assets.Scripts.Core.Models;

namespace Assets.Scripts.Core.GameStates
{
    public class RewardState : DisposableContainer, IState
    {
        private readonly GameModel _gameModel;

        public RewardState(GameModel gameModel)
        {
            _gameModel = gameModel;
        }

        public void OnEnter()
        {
            UnityEngine.Debug.Log("enter reward");
            _gameModel.EndMatch();
            _gameModel.ChangeGameStateTo(GameState.MainMenu);
        }

        public void OnExit()
        {
            UnityEngine.Debug.Log("exit reward");
            Dispose();
        }
    }
}