using Assets.Scripts.Core.GameStates;
using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.Rules;
using System.Collections.Generic;

namespace Assets.Scripts.Core
{
    public interface IGame
    {
        void SwitchStateTo(IState state);
    }

    public enum GameState
    {
        None = 0,
        Boot = 1,
        Loading = 2,
        MainMenu = 3,
        PrepareMatch = 4,
        Match = 5,
        Reward = 6,
        Settings = 7,
        Customization = 8
    }

    public class Game : IGame
    {
        private IState _currentState;
        private ModelStorage _modelStorage;
        private GameRules _gameRules;
        private Dictionary<GameState, IState> _states;

        public Game()
        {
            _states = new Dictionary<GameState, IState>();
            _gameRules = new GameRules();
            _modelStorage = new ModelStorage();
            var gameLoading = new GameLoadingState(_modelStorage, _gameRules, SwitchStateTo);
            RegisterState(GameState.Loading, gameLoading);

            var mainMenuState = new MainMenuState(SwitchStateTo);
            RegisterState(GameState.MainMenu, mainMenuState);

            var prepareMatch = new PrepareMatchState(SwitchStateTo);
            RegisterState(GameState.PrepareMatch, prepareMatch);

            var matchState = new MatchState(SwitchStateTo);
            RegisterState(GameState.Match, matchState);

            var rewardState = new RewardState(SwitchStateTo);
            RegisterState(GameState.Reward, rewardState);

            var settingsState = new SettingsState(SwitchStateTo);
            RegisterState(GameState.Settings, settingsState);

            var customizationState = new CustomizationState(SwitchStateTo);
            RegisterState(GameState.Customization, customizationState);
        }

        public void SwitchStateTo(GameState stateName)
        {
            if (_states.TryGetValue(stateName, out var state))
                SwitchStateTo(state);
            else
                UnityEngine.Debug.Log($"Failed! {stateName} not registered!");
        }

        public void Update()
        {
            if (_currentState is IUpdatableState state)
                state.Update();
        }

        public void SwitchStateTo(IState state)
        {
            if (_currentState != null)
                _currentState.OnExit();

            _currentState = state;
            _currentState.OnEnter();
        }

        public void RegisterState<T>(GameState stateName, T state) where T : IState
        {
            _states[stateName] = state;
        }

    }
}
