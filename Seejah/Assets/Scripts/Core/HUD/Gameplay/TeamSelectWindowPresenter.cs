using Assets.Scripts.Core.Controllers;
using Assets.Scripts.Core.HUD.Elements;
using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.Utils;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Assets.Scripts.Core.HUD
{
    public class TeamSelectWindowPresenter : BaseWindowPresenter
    {
        [SerializeField] private TextMeshProUGUI textDescription;
        [SerializeField] private RawImage imageChipTeam1;
        [SerializeField] private RawImage imageChipTeam2;

        private GameModel _gameModel;
        private UserModel _userModel;
        private AudioService _audioService;
        private TeamType _selectedTeam;

        [Inject]
        public void Construct(GameModel gameModel, UserModel userModel, AudioService audioService)
        {
            _gameModel = gameModel;
            _userModel = userModel;
            _audioService = audioService;
        }

        private void OnStateChange(GameState state)
        {
            gameObject.SetActive(state == GameState.PrepareMatch);
        }

        private void Start()
        {
            CloseAction = CloseWindow;

            _selectedTeam = TeamType.FirstTeam;

            AddForDispose(_gameModel.CurrentGameState.Subscribe(OnStateChange));
            AddForDispose(imageChipTeam1.OnPointerClickAsObservable().Subscribe(_ => OnSelectTeam(TeamType.FirstTeam)));
            AddForDispose(imageChipTeam2.OnPointerClickAsObservable().Subscribe(_ => OnSelectTeam(TeamType.SecondTeam)));
        }

        private void OnSelectTeam(TeamType team)
        {
            _selectedTeam = team;
            CloseWindow();
        }

        private void CloseWindow()
        {
            _audioService.PlayUISound(SoundType.Click);
            _userModel.SetTeam(_selectedTeam);
            _gameModel.StartMatch();
        }
    }
}
