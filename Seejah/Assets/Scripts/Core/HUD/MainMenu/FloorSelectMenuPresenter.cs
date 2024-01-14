using Assets.Scripts.Core.Data.Services;
using Assets.Scripts.Core.HUD.Elements;
using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.SceneInstallers;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Assets.Scripts.Core.HUD
{
    public class FloorSelectMenuPresenter : BaseWindowPresenter
    {
        private const int _SLIDER_MOVE_X = 300;

        [SerializeField] private GoldPanelPresenter goldPanel;
        [SerializeField] private CustomizationItemPresenter itemPrototype;
        [SerializeField] private Transform itemsParent;
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button leftArrowButton;
        [SerializeField] private Button rightArrowButton;

        private UserModel _userModel;
        private CustomizationModel _customizationModel;
        private Func<CustomizationItemPresenter, Transform, CustomizationItemPresenter> _itemFactory;
        private ConfigsStorage _configStorage;
        private ConfirmUnlockWindowPresenter _unlockWindowPresenter;
        private readonly List<CustomizationItemPresenter> _items = new List<CustomizationItemPresenter>();

        [Inject]
        public void Construct(UserModel userModel, CustomizationModel customizationModel, ConfigsStorage configStorage,
                ConfirmUnlockWindowPresenter unlockWindowPresenter,
                Func<CustomizationItemPresenter, Transform, CustomizationItemPresenter> itemFactory)
        {
            _userModel = userModel;
            _customizationModel = customizationModel;
            _configStorage = configStorage;
            _unlockWindowPresenter = unlockWindowPresenter;
            _itemFactory = itemFactory;
        }

        private void OnStateChange(CustomizationState state)
        {
            gameObject.SetActive(state == CustomizationState.FloorCustomization);
        }

        private void Start()
        {
            Title = "Floor";
            CloseAction = OnMenuClose;

            CreateItems();

            AddForDispose(_customizationModel.CurrentState.Subscribe(OnStateChange));
            AddForDispose(_customizationModel.SelectedFloorId.Subscribe(OnItemSelect));
            AddForDispose(confirmButton.OnPointerClickAsObservable().Subscribe(_ => OnMenuClose()));
            AddForDispose(leftArrowButton.OnPointerClickAsObservable().Subscribe(_ => OnLeftClick()));
            AddForDispose(rightArrowButton.OnPointerClickAsObservable().Subscribe(_ => OnRightClick()));

            var boardId = _userModel.SelectedFloorId != 0 ? _userModel.SelectedFloorId : _items.First().DataId;
            SelectFloor(boardId);

            goldPanel.Setup(_userModel);
        }

        private void OnRightClick()
        {
            itemsParent.transform.localPosition += Vector3.left * _SLIDER_MOVE_X;
        }

        private void OnLeftClick()
        {
            itemsParent.transform.localPosition += Vector3.right * _SLIDER_MOVE_X;
        }

        private void OnMenuClose()
        {
            _customizationModel.EndConcreteCustomization();

            var floorId = _configStorage.CustomizationDataList
                    .FirstOrDefault(data => data.Id == _customizationModel.SelectedFloorId.Value)
                    .PrefabId;
            _userModel.ProcessFloorSelection(floorId);
        }

        private void CreateItems()
        {
            var dataList = _customizationModel.FloorDataList;
            for (int i = 0; i < dataList.Count; i++)
            {
                var data = dataList[i];
                var item = _itemFactory.Invoke(itemPrototype, itemsParent);
                item.Setup(data, _userModel.IsItemLocked(data.Id), OnItemClick);
                item.transform.position = Vector3.right * i;
                _items.Add(item);
            }
        }

        private void OnItemClick(int id)
        {
            if (_userModel.IsItemLocked(id))
            {
                _unlockWindowPresenter.ShowConfirm(_customizationModel.FullDataList.FirstOrDefault(m => m.Id == id), OnConfirmUnlock);
                return;
            }
            SelectFloor(id);
        }

        private void OnConfirmUnlock(int id)
        {
            _userModel.UnlockCustomizationItem(id);
            _items.FirstOrDefault(i => i.DataId == id).SetUnlocked();
            OnItemClick(id);
        }

        private void SelectFloor(int id)
        {
            _customizationModel.SelectFloor(id);
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
