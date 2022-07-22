using System;

namespace Assets.Scripts.Core.GameStates
{
    public class PrepareMatchState : IUpdatableState
    {
        private Action<GameState> _switchStateTo;

        public PrepareMatchState(Action<GameState> switchStateTo)
        {
            
            _switchStateTo = switchStateTo;
        }

        public void OnEnter()
        {
            UnityEngine.Debug.Log("enter prepare match");
        }

        public void OnExit()
        {
            UnityEngine.Debug.Log("exit prepare match");
        }

        public void Update()
        {
            UnityEngine.Debug.Log("upd prepare match");
        }
    }
}