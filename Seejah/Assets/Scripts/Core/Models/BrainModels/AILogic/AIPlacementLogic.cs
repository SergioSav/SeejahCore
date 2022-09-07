using Assets.Scripts.Core.Framework;
using Assets.Scripts.Core.Utils;
using System.Linq;

namespace Assets.Scripts.Core.Models.AILogic
{
    public class AIPlacementLogic : DisposableContainer, ILogic
    {
        private readonly FieldModel _fieldModel;
        private readonly RandomProvider _random;

        public AIPlacementLogic(FieldModel fieldModel, RandomProvider random)
        {
            _fieldModel = fieldModel;
            _random = random;
        }

        public CellModel GetCellForMove()
        {
            var availableCells = _fieldModel.Cells
                .Where(c => !c.IsCentral && c.Chip == null)
                .ToList();
            if (_random.GetRandom(availableCells, out var cell))
                return cell;
            return default;
        }
    }
}
