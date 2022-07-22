namespace Assets.Scripts.Core.Models
{
    public class ModelStorage
    {
        private GameModel _gameModel;

        public GameModel GameModel => _gameModel;

        public void UpdateGameModel(GameModel gameModel)
        {
            _gameModel = gameModel;
        }
    }
}