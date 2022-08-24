using System;

namespace Assets.Scripts.Core.Commands
{
    public interface ISwitchStateCommand
    {
        void Execute(GameState stateName);
    }

    [Obsolete]
    public class SwitchStateCommand : ISwitchStateCommand
    {
        private readonly IGame _game;

        public SwitchStateCommand(IGame game)
        {
            _game = game;
        }

        public void Execute(GameState stateName)
        {
            //_game.SwitchStateTo(stateName);
        }
    }
}