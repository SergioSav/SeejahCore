using Assets.Scripts.Core.Rules;

namespace Assets.Scripts.Core.Models
{
    public class PlayerModel : IPlayerModel
    {
        private readonly TeamType _teamType;
        private readonly GameRules _gameRules;
        private readonly IBrain _brain;

        private int _chipsInGame;
        private FieldModel _fieldModel;
        private bool _readyForGame;

        public TeamType TeamType => _teamType;
        public bool IsHuman => _brain.IsHuman;
        public bool ReadyForBattle => _readyForGame;

        public PlayerModel(TeamType teamType, GameRules gameRules, IBrain brain, FieldModel fieldModel)
        {
            _teamType = teamType;
            _gameRules = gameRules;
            _brain = brain;
            _fieldModel = fieldModel;
        }

        public void MakeTurn()
        {
            if (!_brain.IsHuman)
            {
                if (_brain.TryGetCellForMove(out var cell))
                    SelectCell(cell.RowColPair);
            }
        }

        public void SelectCell(RowColPair rcp)
        {
            _fieldModel.RegisterMoveTo(rcp);
        }

        public void AddChipInGame()
        {
            _chipsInGame++;
            if (_chipsInGame == _gameRules.ChipStartCount)
            {
                _brain.SwitchToBattle();
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

    public interface IPlayerModel
    {
        TeamType TeamType { get; }
        bool IsHuman { get; }
        bool ReadyForBattle { get; }

        void AddChipInGame();
        void RemoveInGameChip();
        bool HasEnoughChipInGame();
        void MakeTurn();
        void SelectCell(RowColPair rcp);
    }
}