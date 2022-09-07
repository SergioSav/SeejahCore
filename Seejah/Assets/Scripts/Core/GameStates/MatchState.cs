using Assets.Scripts.Core.Framework;
using Assets.Scripts.Core.Models;
using UniRx;

namespace Assets.Scripts.Core.GameStates
{
    public class MatchState : DisposableContainer, IState
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
            UnityEngine.Debug.Log("match state entered");

            _matchModel.SwitchStateTo(MatchStateType.WaitChipMove);
            AddForDispose(_matchModel.CurrentState.Subscribe(OnMatchModelStateChange));
        }

        private void OnMatchModelStateChange(MatchStateType state)
        {
            if (state == MatchStateType.EndMatch)
                _gameModel.ChangeGameStateTo(GameState.Reward);
        }

        public void OnExit()
        {
            UnityEngine.Debug.Log("match state exit");
            Dispose();
        }
    }
}
