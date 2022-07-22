using System;

namespace Assets.Scripts.Core.GameStates
{
    public class SettingsState : IUpdatableState
    {
        private Action<GameState> _switchStateTo;

        public SettingsState(Action<GameState> switchStateTo)
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