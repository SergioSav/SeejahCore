using Assets.Scripts.Core.Controllers;
using Assets.Scripts.Core.Framework;
using UniRx;

namespace Assets.Scripts.Core.Models
{
    public class GameModel : DisposableContainer
    {
        private ReactiveProperty<GameState> _currentState;
        private IPlayerModel _currentWinner;
        private bool _isRandomPlacementPhase;
        private bool _needUseUltimateAI;

        public IPlayerModel LastWinner => _currentWinner;
        public bool IsRandomPlacementPhase => _isRandomPlacementPhase;
        public bool NeedUseUltimateAI => _needUseUltimateAI;

        public GameModel()
        {
            _currentState = AddForDispose(new ReactiveProperty<GameState>());
        }

        public IReadOnlyReactiveProperty<GameState> CurrentGameState => _currentState;

        public void ChangeGameStateTo(GameState gameState)
        {
            UnityEngine.Debug.Log($"GameModel {gameState}");
            _currentState.Value = gameState;
        }

        public void StartMatch()
        {
            ChangeGameStateTo(GameState.PrepareMatch);
        }

        public void StartMatchWithRandomPlacement()
        {
            _isRandomPlacementPhase = true;
            StartMatch();
        }

        public void StartMatchWithUltimateAI()
        {
            _isRandomPlacementPhase = true; // TEMP
            _needUseUltimateAI = true;
            StartMatch();
        }

        public void EndMatch()
        {
            _isRandomPlacementPhase = false;
            _needUseUltimateAI = false;
            ChangeGameStateTo(GameState.MainMenu);
        }

        public void StartCusomization()
        {
            ChangeGameStateTo(GameState.Customization);
        }

        public void EndCustomization()
        {
            ChangeGameStateTo(GameState.MainMenu);
        }

        public void SetWinner(IPlayerModel winner)
        {
            _currentWinner = winner;
        }
    }
}
