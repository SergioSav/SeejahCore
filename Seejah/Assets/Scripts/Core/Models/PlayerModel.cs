using Assets.Scripts.Core.Commands;
using Assets.Scripts.Core.Rules;

namespace Assets.Scripts.Core.Models
{
    public class PlayerModel : IPlayerModel
    {
        private readonly GameRules _gameRules;
        private readonly ISelectCellCommand _selectCellCommand;
        private TeamType _teamType;
        private IBrain _brain;

        private int _chipsInGame;
        private bool _readyForGame;
        private bool _isTurnComplete;

        public TeamType TeamType => _teamType;
        public bool IsHuman => _brain.IsHuman;
        public bool ReadyForBattle => _readyForGame;

        public PlayerModel(GameRules gameRules, ISelectCellCommand selectCellCommand)
        {
            _gameRules = gameRules;
            _selectCellCommand = selectCellCommand;
        }

        public void Setup(TeamType teamType, IBrain brain)
        {
            _teamType = teamType;
            _brain = brain;
        }

        public void MakeTurn()
        {
            _isTurnComplete = false;
            if (_brain is IAIBrain aiBrain)
            {
                aiBrain.ResetLogic();
                if (aiBrain.TryGetCellForSelect(out var cell))
                    SelectCell(cell.RowColPair);
                if (aiBrain.TryGetCellForMove(out cell))
                    SelectCell(cell.RowColPair);
            }
        }

        public void EndTurn()
        {
            _isTurnComplete = true;
        }

        public void SelectCell(RowColPair rcp)
        {
            if (_isTurnComplete) 
                return;

            _selectCellCommand.Execute(rcp);
        }

        public void AddChipInGame()
        {
            _chipsInGame++;
            if (_chipsInGame == _gameRules.ChipStartCount)
            {
                if (_brain is IAIBrain aiBrain)
                    aiBrain.SwitchToBattle();
                _readyForGame = true;
            }
        }

        public void RemoveInGameChip()
        {
            _chipsInGame--;
        }

        public bool HasEnoughChipInGame()
        {
            return _chipsInGame >= _gameRules.MinimalChipCountInGame;
        }
    }
}