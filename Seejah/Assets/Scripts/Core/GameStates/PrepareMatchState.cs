using Assets.Scripts.Core.Framework;
using Assets.Scripts.Core.Models;
using UniRx;

namespace Assets.Scripts.Core.GameStates
{
    public class PrepareMatchState : DisposableContainer, IState
    {
        private GameModel _gameModel;
        private readonly MatchModel _matchModel;

        public PrepareMatchState(GameModel gameModel, MatchModel matchModel)
        {
            _gameModel = gameModel;
            _matchModel = matchModel;
        }

        public void OnEnter()
        {
            UnityEngine.Debug.Log("enter prepare match");

            AddForDispose(_matchModel.CurrentState.Subscribe(OnMatchModelStateChange));
        }

        private void OnMatchModelStateChange(MatchStateType state)
        {
            if (state == MatchStateType.ReadyForBattle)
                _gameModel.ChangeGameStateTo(GameState.Match);
        }

        public void OnExit()
        {
            UnityEngine.Debug.Log("exit prepare match");
            Dispose();
        }
    }
}