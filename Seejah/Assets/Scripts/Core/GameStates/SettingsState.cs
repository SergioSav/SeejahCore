﻿using Assets.Scripts.Core.Models;
using System;

namespace Assets.Scripts.Core.GameStates
{
    public class SettingsState : IUpdatableState
    {
        private GameModel _gameModel;

        public SettingsState(GameModel gameModel)
        {
            _gameModel = gameModel;
        }

        public void OnEnter()
        {
            //_gameModel.ChangeGameStateTo(GameState.MainMenu);
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