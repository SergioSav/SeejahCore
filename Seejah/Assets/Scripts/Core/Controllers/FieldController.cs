using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.Rules;
using Assets.Scripts.Core.Views;
using System;

namespace Assets.Scripts.Core.Controllers
{
    public class FieldController
    {
        private IFieldView _fieldView;
        private readonly GameRules _rules;
        private readonly FieldModel _fieldModel;

        private TeamType _currentTeamType;

        public FieldController(GameRules rules)
        {
            _rules = rules;

            //_fieldModel = new FieldModel();
            //_fieldModel.AddOnChangeSubscription(OnFieldModelChanged);
            //_fieldModel.AddOnAddSubscription(OnFieldModelAdd);

            //_fieldModel.CreateField(_rules.RowCount, _rules.ColCount);
        }

        

        public void SetView(IFieldView fieldView)
        {
            _fieldView = fieldView;
            _fieldView.SetCellSelectCallback(OnFieldCellSelect);
            OnFieldModelChanged();
        }

        public void SetCurrentTeam(TeamType teamType)
        {
            _currentTeamType = teamType;
        }

        private void OnFieldModelChanged()
        {
            if (_fieldView != null)
                _fieldView.UpdateCells(_fieldModel.Cells);
        }
        private void OnFieldModelAdd(CellModel cell)
        {
            if (_fieldView != null)
                _fieldView.AddChip(cell);
        }

        private void OnFieldCellSelect(int row, int col)
        {
            _fieldModel.TryGenerateChip(row, col, _currentTeamType);
        }
    }
}
