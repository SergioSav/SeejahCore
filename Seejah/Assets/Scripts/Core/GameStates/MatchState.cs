using Assets.Scripts.Core.Models;
using System;

namespace Assets.Scripts.Core.GameStates
{
    public class MatchState : IUpdatableState
    {
        private readonly GameModel _gameModel;
        private readonly MatchModel _matchModel;

        public MatchState(GameModel gameModel, MatchModel matchModel)
        {
            _gameModel = gameModel;
            _matchModel = matchModel;
        }

        public void OnEnter()
        {
            //_gameModel.ChangeGameStateTo(GameState.Reward);
            UnityEngine.Debug.Log("match state entered");

            _matchModel.SwitchStateTo(MatchStateType.WaitChipMove);
        }

        public void OnExit()
        {
            UnityEngine.Debug.Log("match state exit");
        }

        public void Update()
        {
            
        }
    }
}
