using Assets.Scripts.Core.Controllers;
using Assets.Scripts.Core.HUD.Elements;
using Assets.Scripts.Core.Models;
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
        private TeamType _selectedTeam;

        [Inject]
        public void Construct(GameModel gameModel, UserModel userModel)
        {
            _gameModel = gameModel;
            _userModel = userModel;
        }

        private void OnStateChange(GameState state)
        {
            gameObject.SetActive(state == GameState.PrepareMatch);
        }

        private void Start()
        {
            Title = "Chip selection";
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
            _userModel.SetTeam(_selectedTeam);
            _gameModel.StartMatch();
        }
    }
}
