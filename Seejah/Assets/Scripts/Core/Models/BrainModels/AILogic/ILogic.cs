namespace Assets.Scripts.Core.Models.AILogic
{
    public interface ILogic
    {
        CellModel CellForMove();
        CellModel CellForSelect();
        void Reset();
    }
}
