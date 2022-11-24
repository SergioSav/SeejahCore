using Assets.Scripts.Core.SceneInstallers;

namespace Assets.Scripts.Core.Commands
{
    public interface ISelectCustomizationItemCommand
    {
        void Execute(int id);
    }

    public class SelectCustomizationItemCommand : ISelectCustomizationItemCommand
    {
        private readonly CustomizationModel _model;

        public SelectCustomizationItemCommand(CustomizationModel model)
        {
            _model = model;
        }

        public void Execute(int id)
        {
            _model.SelectItem(id);
        }
    }
}
