using Assets.Scripts.Core.Data;
using Assets.Scripts.Core.Framework;
using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.Rules;
using Assets.Scripts.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using VContainer.Unity;

namespace Assets.Scripts.Core.Controllers
{
    public interface IMatch : IStartable
    {
        void SelectCell(RowColPair rcp);
    }

    public class Match : DisposableContainer, IMatch
    {
        private readonly UserModel _userModel;
        private readonly ITimeService _timeService;
        private readonly GameRules _gameRules;
        private readonly GameModel _gameModel;
        private readonly IGameSettings _gameSettings;
        private readonly MatchModel _matchModel;
        private readonly Func<TeamType, IBrain, IPlayerModel> _playerFactory;
        private readonly FieldModel _fieldModel;
        private readonly RandomProvider _random;

        private int _placementChipCount;

        public Match(UserModel userModel, ITimeService timeService, GameRules gameRules, GameModel gameModel, IGameSettings gameSettings,
            MatchModel matchModel, FieldModel fieldModel, RandomProvider random, 
            Func<TeamType, IBrain, IPlayerModel> playerFactory)
        {
            _userModel = userModel;
            _timeService = timeService;
            _gameRules = gameRules;
            _gameModel = gameModel;
            _gameSettings = gameSettings;
            _matchModel = matchModel;
            _fieldModel = fieldModel;
            _random = random;
            _playerFactory = playerFactory;
        }

        public void Start()
        {
            _userModel.SetTeam(TeamType.FirstTeam);
            var player1 = _playerFactory.Invoke(TeamType.FirstTeam, new HumanBrainModel());

            IAIBrain AIBrain = new AIBrainModel(_gameRules, _fieldModel, _random, TeamType.SecondTeam);
            if (_gameSettings.NeedUseUltimateAI)
                AIBrain = new AIUltimateBrainModel(_gameRules, _fieldModel, _random, TeamType.SecondTeam);
            var player2 = _playerFactory.Invoke(TeamType.SecondTeam, AIBrain);

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
                        _timeService.Wait(2)
                            .Then(() =>
                            {
                                var attackingCells = _fieldModel.DetermineAttackThreesome(rcp.Row, rcp.Col, currentPlayer.TeamType);
                                HandleChipAttack(attackingCells);
                                //_fieldModel.PrintFieldForDebug();
                                HandleEndTurn(attackingCells.Count);
                            });
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
                    PlacementPhaseHandle();
                    break;
                case MatchStateType.PlacementDone:
                    _timeService.Wait(1)
                        .Then(_matchModel.StartBattle);
                    break;
                case MatchStateType.PhaseBattle:
                    BattlePhaseHandle();
                    break;
                case MatchStateType.BattleEnd:
                    ProcessBattleEnd();
                    break;
            }
        }

        private void ProcessBattleEnd()
        {
            var winner = _matchModel.GetWinner();
            //_matchModel.Dispose();
            //_fieldModel.Dispose();
            _timeService.Wait(1)
                .Then(() => {
                    _gameModel.SetWinner(winner);
                    if (winner.TeamType == _userModel.TeamType)
                        _userModel.ProcessWin();
                    else
                        _userModel.ProcessLose();
                    _gameModel.ChangeGameStateTo(GameState.Reward);
                }); // TODO:
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
            if (_gameSettings.IsRandomPlacementPhase)
                RandomPlacement();
            else
                _matchModel.ActivePlayer.MakeTurn();
        }

        private void RandomPlacement()
        {
            var currentPlayer = _matchModel.ActivePlayer;
            for (int i = 0; i < _gameRules.ChipStartCount; i++)
            {
                var availableCells = _fieldModel.Cells
                    .Where(c => !c.IsCentral && c.Chip == null)
                    .ToList();
                if (_random.GetRandom(availableCells, out var cell))
                {
                    _fieldModel.TryGenerateChip(cell.RowColPair.Row, cell.RowColPair.Col, currentPlayer.TeamType);
                    _matchModel.ActivePlayer.AddChipInGame();
                }
            }

            _matchModel.ActivePlayer.EndTurn();
            _matchModel.ChooseNextPlayer();
            _matchModel.HandleEndTurn();
        }

        private void BattlePhaseHandle()
        {
            _matchModel.ActivePlayer.MakeTurn();
        }

        private void HandleChipAttack(List<AttackThreesome> attackingCells)
        {
            foreach (var cell in attackingCells)
            {
                _matchModel.HandleRemoveChip(cell.Victim.Chip.Team);
            }
            _fieldModel.HandleChipAttack(attackingCells);
        }

        private void HandleEndTurn(int timeout = 1)
        {
            if (_matchModel.CurrentState.Value == MatchStateType.PhasePlacement)
            {
                _placementChipCount++;
                _matchModel.ActivePlayer.AddChipInGame();
                if (_placementChipCount == _gameRules.ChipPlacementCount)
                {
                    _placementChipCount = 0;
                    _matchModel.ActivePlayer.EndTurn();
                    _timeService.Wait(timeout)
                        .Then(() =>
                        {
                            _matchModel.ChooseNextPlayer();
                            _matchModel.HandleEndTurn();
                        });
                }
                else
                {
                    _timeService.WaitRandom(timeout, timeout + 2)
                        .Then(_matchModel.ActivePlayer.MakeTurn);
                    // TODO: ignore for human
                }
            }
            else if (_matchModel.CurrentState.Value == MatchStateType.PhaseBattle)
            {
                _matchModel.ActivePlayer.EndTurn();
                _timeService.Wait(timeout)
                    .Then(() =>
                    {
                        _matchModel.ChooseNextPlayer();
                        _matchModel.HandleEndTurn();
                    });
            }
        }
    }
}
