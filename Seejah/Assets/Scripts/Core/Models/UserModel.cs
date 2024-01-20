using Assets.Scripts.Core.Data;
using Assets.Scripts.Core.Data.Services;
using Assets.Scripts.Core.Framework;
using Assets.Scripts.Core.SceneInstallers;
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

        public ReactiveProperty<int> CurrentGold;

        public UserModel(ISaveService saveService, CustomizationModel customizationModel)
        {
            _saveService = saveService;
            _customizationModel = customizationModel;

            CurrentGold = AddForDispose(new ReactiveProperty<int>());

            TryLoadSaveState();
        }

        private void TryLoadSaveState()
        {
            var state = _saveService.Load();
            if (state == null)
            {
                TryGenerateNewUser();
                return;
            }

            Name = state.Name;
            Id = state.Id;
            WinCount = state.WinCount;
            WinStreak = state.WinStreak;
            LoseCount = state.LoseCount;
            LoseStreak = state.LoseStreak;
            SelectedChipId = state.SelectedChipId;
            SelectedChipColorId = state.SelectedChipColorId;
            SelectedBoardId = state.SelectedBoardId;
            SelectedFloorId = state.SelectedFloorId;
            UnlockedCustomizationItems = state.UnlockedCustomizationItems;
            CurrentGold.Value = state.CurrentGold;
        }

        private void TryGenerateNewUser()
        {
            Name = "unknown";
            Id = 999;
            SelectedChipId = 1;
            SelectedChipColorId = 6;
            SelectedBoardId = 11;
            SelectedFloorId = 19;
            UnlockedCustomizationItems = _customizationModel.FullDataList.Where(m => m.Price.Value <= 0).Select(m => m.Id).ToList();
            CurrentGold.Value = 0;

            TrySaveState();
        }

        private void TrySaveState()
        {
            var state = new UserSaveState
            {
                Name = Name,
                Id = Id,
                WinCount = WinCount,
                WinStreak = WinStreak,
                LoseCount = LoseCount,
                LoseStreak = LoseStreak,
                SelectedChipId = SelectedChipId,
                SelectedChipColorId = SelectedChipColorId,
                SelectedBoardId = SelectedBoardId,
                SelectedFloorId = SelectedFloorId,
                UnlockedCustomizationItems = UnlockedCustomizationItems,
                CurrentGold = CurrentGold.Value
            };
            _saveService.Save(state);
        }

        public void SetTeam(TeamType teamType)
        {
            _teamType = teamType;
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