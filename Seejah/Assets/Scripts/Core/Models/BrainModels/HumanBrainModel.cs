using Assets.Scripts.Core.Framework;

namespace Assets.Scripts.Core.Models
{
    public class HumanBrainModel : DisposableContainer, IBrain
    {
        private readonly FieldModel _fieldModel;

        public HumanBrainModel(FieldModel fieldModel)
        {
            _fieldModel = fieldModel;
        }

        public bool IsHuman => true;

        public void Reset()
        {
        }

        public void SwitchToBattle()
        {
        }

        public void SwitchToPlacement()
        {
        }

        public bool TryGetCellForMove(out CellModel cell)
        {
            cell = default;
            return false;
        }
    }
}
