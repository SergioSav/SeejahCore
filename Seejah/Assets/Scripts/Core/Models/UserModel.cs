using Assets.Scripts.Core.Data;
using Assets.Scripts.Core.Data.Services;
using Assets.Scripts.Core.Framework;
using Assets.Scripts.Core.SceneInstallers;
using Assets.Scripts.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;

namespace Assets.Scripts.Core.Models
{
    public class UserModel : DisposableContainer
    {
        private readonly ISaveService _saveService;
        private readonly CustomizationModel _customizationModel;
        private readonly IPlatformService _platformService;
        public string Name;
        public int Id;

        private TeamType _teamType;
        public TeamType TeamType => _teamType;
        public TeamType OpponentTeamType => _teamType == TeamType.FirstTeam ? TeamType.SecondTeam : TeamType.FirstTeam;

        public int WinCount { get; private set; }
        public int WinStreak { get; private set; }
        public int LoseCount { get; private set; }
        public int LoseStreak { get; private set; }
        public int SelectedChipId { get; private set; }
        public int SelectedChipColorId { get; private set; }
        public int SelectedBoardId { get; private set; }
        public int SelectedFloorId { get; private set; }
        public List<int> UnlockedCustomizationItems { get; private set; }
        public bool WasTutorialShown { get; private set; }
        public int RatingScore { get; private set; }

        public ReactiveProperty<int> CurrentGold;
        private PlayerSaveState _playerSaveState;

        public UserModel(ISaveService saveService, CustomizationModel customizationModel, IPlatformService platformService)
        {
            _saveService = saveService;
            _customizationModel = customizationModel;
            _platformService = platformService;

            CurrentGold = AddForDispose(new ReactiveProperty<int>());

            TryLoadSaveState();

            SendMetrics();
        }

        private void SendMetrics()
        {
            var param = GetMetricsInfo();
            _platformService.SendMetric(MetricsConst.UserInfo, param);
        }

        private void TryLoadSaveState()
        {
            _saveService.Load();
            _playerSaveState = _saveService.GetPlayerSaveState();
            if (_playerSaveState.Id < 0)
            {
                TryGenerateNewUser();
                return;
            }

            Name = _playerSaveState.Name;
            Id = _playerSaveState.Id;
            WinCount = _playerSaveState.WinCount;
            WinStreak = _playerSaveState.WinStreak;
            LoseCount = _playerSaveState.LoseCount;
            LoseStreak = _playerSaveState.LoseStreak;
            RatingScore = _playerSaveState.RatingScore;
            SelectedChipId = _playerSaveState.SelectedChipId;
            SelectedChipColorId = _playerSaveState.SelectedChipColorId;
            SelectedBoardId = _playerSaveState.SelectedBoardId;
            SelectedFloorId = _playerSaveState.SelectedFloorId;
            UnlockedCustomizationItems = _playerSaveState.UnlockedCustomizationItems;
            CurrentGold.Value = _playerSaveState.CurrentGold;
            WasTutorialShown = _playerSaveState.WasTutorialShown;
        }

        private void TryGenerateNewUser()
        {
            Name = "unknown";
            Id = 999;
            RatingScore = 1000;
            SelectedChipId = 1;
            SelectedChipColorId = 6;
            SelectedBoardId = 11;
            SelectedFloorId = 19;
            UnlockedCustomizationItems = _customizationModel.FullDataList.Where(m => m.Price.Value <= 0).Select(m => m.Id).ToList();
            CurrentGold.Value = 0;
            WasTutorialShown = false;

            TrySaveState();
        }

        private void TrySaveState()
        {
            _playerSaveState.Name = Name;
            _playerSaveState.Id = Id;
            _playerSaveState.WinCount = WinCount;
            _playerSaveState.WinStreak = WinStreak;
            _playerSaveState.LoseCount = LoseCount;
            _playerSaveState.LoseStreak = LoseStreak;
            _playerSaveState.RatingScore = RatingScore;
            _playerSaveState.SelectedChipId = SelectedChipId;
            _playerSaveState.SelectedChipColorId = SelectedChipColorId;
            _playerSaveState.SelectedBoardId = SelectedBoardId;
            _playerSaveState.SelectedFloorId = SelectedFloorId;
            _playerSaveState.UnlockedCustomizationItems = UnlockedCustomizationItems;
            _playerSaveState.CurrentGold = CurrentGold.Value;
            _playerSaveState.WasTutorialShown = WasTutorialShown;

            _saveService.Save(_playerSaveState);
        }

        private Dictionary<string, string> GetMetricsInfo()
        {
            return new Dictionary<string, string>
            {
                { MetricsConst.UserWinCount, WinCount.ToString() },
                { MetricsConst.UserLoseCount, LoseCount.ToString() },
                { MetricsConst.UserRating, RatingScore.ToString() },
                { MetricsConst.UserCoins, CurrentGold.ToString() },
                { MetricsConst.UserAppearanceBoardId, SelectedBoardId.ToString() },
                { MetricsConst.UserAppearanceChipId, SelectedChipId.ToString() },
                { MetricsConst.UserAppearanceColorId, SelectedChipColorId.ToString() },
                { MetricsConst.UserAppearanceFloorId, SelectedFloorId.ToString() },
            };
        }

        public void SetTeam(TeamType teamType)
        {
            _teamType = teamType;
        }

        public void ProcessTutorialShow()
        {
            WasTutorialShown = true;
            TrySaveState();
        }

        public void ProcessWin()
        {
            WinCount++;
            if (LoseStreak != 0)
                LoseStreak = 0;
            WinStreak++;
            TrySaveState();
        }

        public void ProcessLose()
        {
            LoseCount++;
            if (WinStreak != 0)
                WinStreak = 0;
            LoseStreak++;
            TrySaveState();
        }

        public void ProcessChipSelection(int id)
        {
            SelectedChipId = id;
            TrySaveState();
        }

        public void ProcessChipColorSelection(int id)
        {
            SelectedChipColorId = id;
            TrySaveState();
        }

        public void ProcessBoardSelection(int id)
        {
            SelectedBoardId = id;
            TrySaveState();
        }

        public void ProcessFloorSelection(int id)
        {
            SelectedFloorId = id;
            TrySaveState();
        }

        public void UnlockCustomizationItem(int id)
        {
            UnlockedCustomizationItems.Add(id);
            TrySaveState();
        }

        public bool IsItemLocked(int id)
        {
            return !UnlockedCustomizationItems.Contains(id);
        }

        public void ChangeRatingScore(int diff)
        {
            RatingScore += diff;
        }

        public void AddGold(int amount)
        {
            CurrentGold.Value += amount;
        }

        public void ReduceGold(int amount)
        {
            CurrentGold.Value -= amount;
        }

        public void ApplyDrop(DropData drop)
        {
            if (drop.Currency == CurrencyType.Gold)
                AddGold(drop.Value);
        }
    }
}