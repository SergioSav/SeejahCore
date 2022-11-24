using Assets.Scripts.Core.Data.Services;

namespace Assets.Scripts.Core.Models
{
    public class UserModel
    {
        private readonly ISaveService _saveService;

        public string Name;
        public int Id;

        private TeamType _teamType;
        public TeamType TeamType => _teamType;

        public int WinCount { get; private set; }
        public int WinStreak { get; private set; }
        public int LoseCount { get; private set; }
        public int LoseStreak { get; private set; }
        public int SelectedChipId { get; private set; }

        public UserModel(ISaveService saveService)
        {
            _saveService = saveService;

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
        }

        private void TryGenerateNewUser()
        {
            Name = "unknown";
            Id = 999;
            SelectedChipId = 1;
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
                SelectedChipId = SelectedChipId
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
    }
}