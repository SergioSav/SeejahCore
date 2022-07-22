using Assets.Scripts.Core.GameStates;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Core
{
    public class BootScript : MonoBehaviour, IBootView
    {
        private Game _game;

        void Start()
        {
            _game = new Game();

            var bootState = new BootState(_game, this, _game.SwitchStateTo);
            _game.RegisterState(GameState.Boot, bootState);
            _game.SwitchStateTo(GameState.Boot);

            DontDestroyOnLoad(this);
        }

        public void Update()
        {
            _game.Update();
        }

        public void SwitchScene()
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

    public interface IBootView
    {
        void SwitchScene();
    }
}