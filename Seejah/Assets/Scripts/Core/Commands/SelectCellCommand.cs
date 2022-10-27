using Assets.Scripts.Core.Controllers;
using Assets.Scripts.Core.Models;

namespace Assets.Scripts.Core.Commands
{
    public interface ISelectCellCommand
    {
        void Execute(RowColPair rcp);
    }

    public class SelectCellCommand : ISelectCellCommand
    {
        private readonly IMatch _match;

        public SelectCellCommand(IMatch match)
        {
            _match = match;
        }

        public void Execute(RowColPair rcp)
        {
            _match.SelectCell(rcp);
        }
    }
}
