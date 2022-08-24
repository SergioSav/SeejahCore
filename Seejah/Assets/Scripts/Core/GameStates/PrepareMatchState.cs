using Assets.Scripts.Core.Controllers;
using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.Rules;

namespace Assets.Scripts.Core.GameStates
{
    public class PrepareMatchState : IUpdatableState
    {
        private readonly IGame _game;
        private GameModel _gameModel;
        private readonly GameRules _gameRules;
        private FieldController _fieldController;

        public PrepareMatchState(IGame game, GameRules gameRules, GameModel gameModel)
        {
            _game = game;
            _gameModel = gameModel;
            _gameRules = gameRules;
        }

        public void OnEnter()
        {
            UnityEngine.Debug.Log("enter prepare match");
            CreateField();

            //_gameModel.ChangeGameStateTo(GameState.Match);
        }

        private void CreateField()
        {
            _fieldController = new FieldController(_gameRules);
            //_fieldController.SetView(_game.GetFieldView());
        }

        public void OnExit()
        {
            UnityEngine.Debug.Log("exit prepare match");
        }

        public void Update()
        {
            
        }
    }
}