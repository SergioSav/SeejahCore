using System;

namespace Assets.Scripts.Core.GameStates
{
    public class BootState : IState
    {
        private readonly IGame _game;
        private IBootView _bootView;
        private readonly Action<GameState> _switchOnDone;

        public BootState(IGame game, IBootView bootView, Action<GameState> switchOnDone)
        {
            _game = game;
            _bootView = bootView;
            _switchOnDone = switchOnDone;
        }

        public void OnEnter()
        {
            UnityEngine.Debug.Log("enter boot");
            // init smthng
            _bootView.SwitchScene();
            _switchOnDone.Invoke(GameState.Loading);
        }

        public void OnExit()
        {
            UnityEngine.Debug.Log("exit boot");
        }
    }
}
