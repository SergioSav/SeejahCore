﻿namespace Assets.Scripts.Core.Rules
{
    public class GameRules
    {
        private int _fieldRowCount;
        private int _fieldColCount;
        private int _chipPlacementCount;
        private int _chipStartCount;

        public GameRules()
        {
            _fieldRowCount = 5;
            _fieldColCount = 5;
            _chipPlacementCount = 2;
            _chipStartCount = 12;
        }
    }
}