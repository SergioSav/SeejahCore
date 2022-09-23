﻿using Assets.Scripts.Core.Framework;
using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.Rules;
using Assets.Scripts.Core.Utils;
using System;
using System.Collections.Generic;
using UniRx;
using VContainer.Unity;

namespace Assets.Scripts.Core
{
    public interface IMatch : IStartable
    {
        void SelectCell(RowColPair rcp);
    }

    public class Match : DisposableContainer, IMatch
    {
        private readonly ITimeService _timeService;
        private readonly GameRules _gameRules;
        private readonly MatchModel _matchModel;
        private readonly Func<TeamType, IBrain, IPlayerModel> _playerFactory;
        private readonly FieldModel _fieldModel;
        private readonly RandomProvider _random;

        private int _placementChipCount;

        public Match(ITimeService timeService, GameRules gameRules, MatchModel matchModel, FieldModel fieldModel, RandomProvider random,
            Func<TeamType, IBrain, IPlayerModel> playerFactory)
        {
            _timeService = timeService;
            _gameRules = gameRules;
            _matchModel = matchModel;
            _fieldModel = fieldModel;
            _random = random;
            _playerFactory = playerFactory;
        }

        public void Start()
        {
            var player1 = _playerFactory.Invoke(TeamType.TeamRed, new HumanBrainModel());
            var player2 = _playerFactory.Invoke(TeamType.TeamBlue, new AIBrainModel(_gameRules, _fieldModel, _random, TeamType.TeamBlue));

            _matchModel.AddPlayers(new List<IPlayerModel> { player1, player2 });
            _matchModel.ChooseFirstPlayer();

            _fieldModel.CreateField(_gameRules.RowCount, _gameRules.ColCount);

            AddForDispose(_matchModel.CurrentState.Subscribe(OnMatchStateChange));
            AddForDispose(_matchModel.WaitNextTurn.Subscribe(_ => OnWaitNextTurn()));

            _matchModel.SetLoading();
        }


        public void SelectCell(RowColPair rcp)
        {
            var currentPlayer = _matchModel.ActivePlayer;
            if (_matchModel.CurrentState.Value == MatchStateType.PhasePlacement)
            {
                var isPlaceSuccess = _fieldModel.TryGenerateChip(rcp.Row, rcp.Col, currentPlayer.TeamType);
                if (isPlaceSuccess)
                    HandleEndTurn();
            }
            else if (_matchModel.CurrentState.Value == MatchStateType.PhaseBattle)
            {
                if (_fieldModel.SelectedCell != null)
                {
                    var isMoveSuccess = _fieldModel.TryMoveSelectedChip(rcp.Row, rcp.Col);
                    if (isMoveSuccess)
                    {
                        var updatingCells = _fieldModel.DetermineOpponentsChipForRemove(rcp.Row, rcp.Col, currentPlayer.TeamType);
                        HandleChipRemove(updatingCells);
                        //_fieldModel.PrintFieldForDebug();
                        HandleEndTurn();
                    }
                    else
                    {
                        _fieldModel.TrySelectChip(rcp.Row, rcp.Col, currentPlayer.TeamType);
                    }
                }
                else
                {
                    _fieldModel.TrySelectChip(rcp.Row, rcp.Col, currentPlayer.TeamType);
                }
            }
        }

        private void OnMatchStateChange(MatchStateType state)
        {
            switch (state)
            {
                case MatchStateType.Loading:
                    _timeService.Wait(1)
                        .Then(_matchModel.SetReady);
                    break;
                case MatchStateType.Ready:
                    _timeService.Wait(1)
                        .Then(_matchModel.StartPlacement);
                    break;
                case MatchStateType.PhasePlacement:
                    break;
                case MatchStateType.PlacementDone:
                    _timeService.Wait(1)
                        .Then(_matchModel.StartBattle);
                    break;
                case MatchStateType.PhaseBattle:
                    break;
                case MatchStateType.BattleEnd:
                    break;
            }
        }

        private void OnWaitNextTurn()
        {
            if (_matchModel.CurrentState.Value == MatchStateType.PhasePlacement)
                PlacementPhaseHandle();
            else if (_matchModel.CurrentState.Value == MatchStateType.PhaseBattle)
                BattlePhaseHandle();
        }

        private void PlacementPhaseHandle()
        {
            _matchModel.ActivePlayer.MakeTurn();
        }

        private void BattlePhaseHandle()
        {
            _matchModel.ActivePlayer.MakeTurn();
        }

        private void HandleChipRemove(List<CellModel> cells)
        {
            foreach (var cell in cells)
            {
                _matchModel.HandleRemoveChip(cell.Chip.Team);
                _fieldModel.HandleRemoveChip(cell);
            }
        }

        private void HandleEndTurn()
        {
            if (_matchModel.CurrentState.Value == MatchStateType.PhasePlacement)
            {
                _placementChipCount++;
                _matchModel.ActivePlayer.AddChipInGame();
                if (_placementChipCount == _gameRules.ChipPlacementCount)
                {
                    _placementChipCount = 0;
                    _matchModel.ActivePlayer.EndTurn();
                    _timeService.Wait(2)
                        .Then(() =>
                        {
                            _matchModel.ChooseNextPlayer();
                            _matchModel.HandleEndTurn();
                        });
                }
                else
                {
                    _timeService.WaitRandom(1, 3)
                        .Then(_matchModel.ActivePlayer.MakeTurn);
                    // TODO: ignore for human
                }
            }
            else if (_matchModel.CurrentState.Value == MatchStateType.PhaseBattle)
            {
                _matchModel.ActivePlayer.EndTurn();
                _timeService.Wait(1)
                    .Then(() =>
                    {
                        _matchModel.ChooseNextPlayer();
                        _matchModel.HandleEndTurn();
                    });
            }
        }
    }
}