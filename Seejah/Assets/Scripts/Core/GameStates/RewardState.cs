using System;

namespace Assets.Scripts.Core.GameStates
{
    public class RewardState : IUpdatableState
    {
        private Action<GameState> _switchStateTo;

        public RewardState(Action<GameState> switchStateTo)
        {
            
            _switchStateTo = switchStateTo;
        }

        public void OnEnter()
        {
            throw new NotImplementedException();
        }

        public void OnExit()
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}