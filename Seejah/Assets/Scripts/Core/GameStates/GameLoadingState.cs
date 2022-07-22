using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.Rules;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Core.GameStates
{
    public class GameLoadingState : IUpdatableState
    {
        private ModelStorage _modelStorage;
        private GameRules _gameRules;
        private readonly Action<GameState> _switchOnDone;

        public GameLoadingState(ModelStorage modelStorage, GameRules gameRules, Action<GameState> switchOnDone)
        {
            _modelStorage = modelStorage;
            _gameRules = gameRules;
            _switchOnDone = switchOnDone;
        }

        public void OnEnter()
        {
            UnityEngine.Debug.Log("enter loading");
            CreateModels();

            _switchOnDone.Invoke(GameState.MainMenu);
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
            var players = new List<PlayerModel> { new PlayerModel(1), new PlayerModel(2) };
            var gameModel = new GameModel(_gameRules, players);
            _modelStorage.UpdateGameModel(gameModel);
        }

    }
}
