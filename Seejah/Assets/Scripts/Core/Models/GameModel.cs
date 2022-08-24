using Assets.Scripts.Core.Framework;
using Assets.Scripts.Core.Rules;
using System.Collections.Generic;
using UniRx;

namespace Assets.Scripts.Core.Models
{
    public class GameModel : DisposableContainer
    {
        private ReactiveProperty<GameState> _currentState;
        private ReactiveProperty<bool> _needStartMatch;

        private readonly GameRules _rules;

        private List<PlayerModel> _players;

        public GameModel(GameRules rules)
        {
            _rules = rules;
            _currentState = AddForDispose(new ReactiveProperty<GameState>());
            _needStartMatch = AddForDispose(new ReactiveProperty<bool>());
        }

        public IReadOnlyReactiveProperty<GameState> CurrentGameState => _currentState;
        public IReadOnlyReactiveProperty<bool> NeedStartMatch => _needStartMatch;

        public void ChangeGameStateTo(GameState gameState)
        {
            UnityEngine.Debug.Log($"GameModel {gameState}");
            _currentState.Value = gameState;
        }

        public void AddPlayers(List<PlayerModel> players)
        {
            _players = players;
        }

        public void StartMatch()
        {
            _needStartMatch.Value = true;
        }
    }
}
