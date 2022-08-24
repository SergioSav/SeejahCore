using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.Rules;
using System.Collections.Generic;

namespace Assets.Scripts.Core.GameStates
{
    public class GameLoadingState : IUpdatableState
    {
        private readonly GameRules _gameRules;
        private readonly GameModel _gameModel;

        public GameLoadingState(GameRules gameRules, GameModel gameModel)
        {
            _gameRules = gameRules;
            _gameModel = gameModel;
        }

        public void OnEnter()
        {
            UnityEngine.Debug.Log("enter loading");
            CreateModels();

            _gameModel.ChangeGameStateTo(GameState.MainMenu);
        }

        public void OnExit()
        {
            UnityEngine.Debug.Log("exit loading");
        }

        public void Update()
        {
            UnityEngine.Debug.Log("upd loading state");
        }

        private void CreateModels()
        {
            var players = new List<PlayerModel> { new PlayerModel(TeamType.TeamRed), new PlayerModel(TeamType.TeamBlue) };
            _gameModel.AddPlayers(players);
        }
    }
}
