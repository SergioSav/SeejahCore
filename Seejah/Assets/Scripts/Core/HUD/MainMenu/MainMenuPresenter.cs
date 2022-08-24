﻿using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.Presenters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using UniRx;

namespace Assets.Scripts.Core.HUD.MainMenu
{
    public class MainMenuPresenter : MonoBehPresenter
    {
        [SerializeField] private TextMeshProUGUI textTitle;
        [SerializeField] private Button buttonStart;

        private GameModel _gameModel;

        [Inject]
        public void Construct(GameModel gameModel)
        {
            _gameModel = gameModel;
        }

        private void Start()
        {
            AddForDispose(buttonStart
                .OnClickAsObservable()
                .Subscribe(_ => _gameModel.StartMatch()));
        }
    }
}