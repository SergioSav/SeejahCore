using Assets.Scripts.Core.Framework;
using Assets.Scripts.Core.GameStates;
using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.Rules;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace Assets.Scripts.Core
{
    public interface IGame : IStartable, ITickable
    {
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

    public class Game : DisposableContainer, IGame
    {
        private IState _currentState;
        private GameRules _gameRules;
        private LifetimeScope _coreScope;
        private readonly GameModel _gameModel;
        private LifetimeScope _gameScope;

        public Game(LifetimeScope coreScope, GameModel gameModel)
        {
            _coreScope = coreScope;
            _gameModel = gameModel;
        }

        public void Start()
        {
            _gameScope = _coreScope.CreateChild(InstallDependencies);

            AddForDispose(_gameModel.CurrentGameState.Subscribe(OnGameStateChange));
            _gameModel.ChangeGameStateTo(GameState.Boot);
        }

        public void Tick()
        {
            if (_currentState is IUpdatableState state)
                state.Update();
        }

        private void OnGameStateChange(GameState gameState)
        {
            UnityEngine.Debug.Log($"GAME {gameState}");
            SwitchStateTo(gameState);
        }

        private void InstallDependencies(IContainerBuilder builder)
        {
            builder.Register<BootState>(Lifetime.Transient);
            builder.Register<GameLoadingState>(Lifetime.Transient);
            builder.Register<MainMenuState>(Lifetime.Transient);
            builder.Register<PrepareMatchState>(Lifetime.Transient);
            builder.Register<MatchState>(Lifetime.Transient);
            builder.Register<RewardState>(Lifetime.Transient);
            builder.Register<SettingsState>(Lifetime.Transient);
            builder.Register<CustomizationState>(Lifetime.Transient);
        }

        private void SwitchStateTo(GameState stateName)
        {
            IState newState = stateName switch
            {
                GameState.None => null,
                GameState.Boot => _gameScope.Container.Resolve<BootState>(),
                GameState.Loading => _gameScope.Container.Resolve<GameLoadingState>(),
                GameState.MainMenu => _gameScope.Container.Resolve<MainMenuState>(),
                GameState.PrepareMatch => _gameScope.Container.Resolve<PrepareMatchState>(),
                GameState.Match => _gameScope.Container.Resolve<MatchState>(),
                GameState.Reward => _gameScope.Container.Resolve<RewardState>(),
                GameState.Settings => _gameScope.Container.Resolve<SettingsState>(),
                GameState.Customization => _gameScope.Container.Resolve<CustomizationState>(),
                _ => throw new System.NotImplementedException()
            };
            if (newState != null)
                SwitchStateTo(newState);
        }

        private void SwitchStateTo(IState state)
        {
            if (_currentState != null)
                _currentState.OnExit();

            _currentState = state;
            _currentState.OnEnter();
        }

        //public IFieldView GetFieldView()
        //{
        //    // TODO: need another impl
        //    var activeScene = SceneManager.GetActiveScene();
        //    var gameObjects = activeScene.GetRootGameObjects();
        //    foreach (var go in gameObjects)
        //    {
        //        var view = go.GetComponent<FieldView>();
        //        if (view != null)
        //            return view;
        //    }
        //    return default;
        //}

        
    }
}
