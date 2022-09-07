using Assets.Scripts.Core.Framework;
using Assets.Scripts.Core.Rules;
using Assets.Scripts.Core.Utils;
using System.Collections.Generic;
using UniRx;

namespace Assets.Scripts.Core.Models
{
    public class GameModel : DisposableContainer
    {
        private ReactiveProperty<GameState> _currentState;
        private ReactiveProperty<bool> _needStartMatch;
        private readonly RandomProvider _randomProvider;
        private readonly GameRules _rules;

        private List<PlayerModel> _players;
        private PlayerModel _currentActivePlayer;

        public GameModel(RandomProvider randomProvider, GameRules rules)
        {
            _randomProvider = randomProvider;
            _rules = rules;

            _currentState = AddForDispose(new ReactiveProperty<GameState>());
            _needStartMatch = AddForDispose(new ReactiveProperty<bool>());
        }

        public IReadOnlyReactiveProperty<GameState> CurrentGameState => _currentState;
        public IReadOnlyReactiveProperty<bool> NeedStartMatch => _needStartMatch;

        public int PlayerCount => _players.Count;
        public List<PlayerModel> Players => _players;

        public void ChangeGameStateTo(GameState gameState)
        {
            UnityEngine.Debug.Log($"GameModel {gameState}");
            _currentState.Value = gameState;
        }

        public void AddPlayers(List<PlayerModel> players)
        {
            _players = players;
        }

        public PlayerModel ChooseFirstPlayer()
        {
            _randomProvider.GetRandom(_players, out _currentActivePlayer);
            return _currentActivePlayer;
        }

        public PlayerModel NextPlayer()
        {
            var index = _players.IndexOf(_currentActivePlayer);
            var nextIndex = (index + 1) % _players.Count;
            _currentActivePlayer = _players[nextIndex];
            return _currentActivePlayer;
        }

        public void StartMatch()
        {
            _needStartMatch.Value = true;
        }

        public void EndMatch()
        {
            _needStartMatch.Value = false;
        }
    }
}
