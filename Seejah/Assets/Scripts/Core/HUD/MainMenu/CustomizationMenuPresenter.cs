using Assets.Scripts.Core.Controllers;
using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.Presenters;
using Assets.Scripts.Core.SceneInstallers;
using System;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Assets.Scripts.Core.HUD
{
    public class CustomizationMenuPresenter : MonoBehPresenter
    {
        [SerializeField] private TextMeshProUGUI textTitle;
        [SerializeField] private Button buttonClose;
        [SerializeField] private CustomizationItemPresenter itemPrototype;
        [SerializeField] private Transform itemsParent;

        private GameModel _gameModel;
        private UserModel _userModel;
        private CustomizationModel _customizationModel;
        private Func<CustomizationItemPresenter, Transform, CustomizationItemPresenter> _itemFactory;
        private List<CustomizationItemPresenter> _items = new List<CustomizationItemPresenter>();

        [Inject]
        public void Construct(GameModel gameModel, UserModel userModel, CustomizationModel customizationModel, 
            Func<CustomizationItemPresenter, Transform, CustomizationItemPresenter> itemFactory)
        {
            _gameModel = gameModel;
            _userModel = userModel;
            _customizationModel = customizationModel;
            _itemFactory = itemFactory;
        }

        private void OnStateChange(GameState state)
        {
            gameObject.SetActive(state == GameState.Customization);
        }

        private void Start()
        {
            CreateItems();

            AddForDispose(_gameModel.CurrentGameState.Subscribe(OnStateChange));
            AddForDispose(buttonClose
                .OnClickAsObservable()
                .Subscribe(_ => OnMenuClose()));
            AddForDispose(_customizationModel.SelectedItem.Subscribe(OnItemSelect));

            if (_userModel.SelectedChipId != 0)
                OnItemSelect(_userModel.SelectedChipId);
        }

        private void OnMenuClose()
        {
            _gameModel.EndCustomization();
            _userModel.ProcessChipSelection(_customizationModel.SelectedItem.Value);
        }

        private void CreateItems()
        {
            for (int i = 0; i < _customizationModel.DataList.Count; i++)
            {
                var data = _customizationModel.DataList[i];
                var item = _itemFactory.Invoke(itemPrototype, itemsParent);
                item.SetData(data);
                item.transform.position = Vector3.right * i;
                _items.Add(item);
            }
        }

        private void OnItemSelect(int id)
        {
            foreach (var item in _items)
            {
                item.SetSelected(id);
            }
        }
    }
}
