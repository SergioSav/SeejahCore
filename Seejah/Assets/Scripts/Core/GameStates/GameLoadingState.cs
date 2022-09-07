using Assets.Scripts.Core.Framework;
using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.Rules;
using Assets.Scripts.Core.Utils;
using System.Collections.Generic;

namespace Assets.Scripts.Core.GameStates
{
    public class GameLoadingState : DisposableContainer, IState
    {
        private readonly GameRules _gameRules;
        private readonly GameModel _gameModel;
        private readonly FieldModel _fieldModel;
        private readonly RandomProvider _random;

        public GameLoadingState(GameRules gameRules, GameModel gameModel, FieldModel fieldModel, RandomProvider random)
        {
            _gameRules = gameRules;
            _gameModel = gameModel;
            _fieldModel = fieldModel;
            _random = random;
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

        private void CreateModels()
        {
            var players = new List<PlayerModel> { 
                new PlayerModel(TeamType.TeamRed, _gameRules, new HumanBrainModel(_fieldModel), _fieldModel), 
                new PlayerModel(TeamType.TeamBlue, _gameRules, new AIBrainModel(_fieldModel, _random, TeamType.TeamBlue), _fieldModel) 
            };
            _gameModel.AddPlayers(players);
        }
    }
}
