using System;

namespace Assets.Scripts.Core.GameStates
{
    public class MainMenuState : IState
    {
        private Action<GameState> _switchStateTo;

        public MainMenuState(Action<GameState> switchStateTo)
        {
            _switchStateTo = switchStateTo;
        }

        public void OnEnter()
        {
            UnityEngine.Debug.Log("enter main menu");
            _switchStateTo.Invoke(GameState.PrepareMatch);
        }

        public void OnExit()
        {
            UnityEngine.Debug.Log("exit main menu");
        }
    }
}
