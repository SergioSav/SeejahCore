using Assets.Scripts.Core.Rules;
using System.Collections.Generic;

namespace Assets.Scripts.Core.Models
{
    public class GameModel
    {
        private readonly GameRules _rules;

        private List<PlayerModel> _players;

        public GameModel(GameRules rules, List<PlayerModel> players)
        {
            _rules = rules;
            _players = players;
        }
    }
}
