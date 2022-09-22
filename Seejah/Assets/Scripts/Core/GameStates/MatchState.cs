using Assets.Scripts.Core.Framework;
using Assets.Scripts.Core.Models;

namespace Assets.Scripts.Core.GameStates
{
    public class MatchState : DisposableContainer, IState
    {
        private readonly GameModel _gameModel;

        public MatchState(GameModel gameModel)
        {
            _gameModel = gameModel;
        }

        public void OnEnter()
        {
            UnityEngine.Debug.Log("match state entered");
        }

        public void OnExit()
        {
            UnityEngine.Debug.Log("match state exit");
            Dispose();
        }
    }
}
