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
    public class BoardSelectMenuPresenter : BaseWindowPresenter
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
            gameObject.SetActive(state == CustomizationState.BoardCustomization);
        }

        private void Start()
        {
            Title = "Boards";
            CloseAction = OnMenuClose;

            CreateItems();

            AddForDispose(_customizationModel.CurrentState.Subscribe(OnStateChange));
            AddForDispose(_customizationModel.SelectedBoardId.Subscribe(OnItemSelect));
            AddForDispose(confirmButton.OnPointerClickAsObservable().Subscribe(_ => OnMenuClose()));
            AddForDispose(leftArrowButton.OnPointerClickAsObservable().Subscribe(_ => OnLeftClick()));
            AddForDispose(rightArrowButton.OnPointerClickAsObservable().Subscribe(_ => OnRightClick()));

            var boardId = _userModel.SelectedBoardId != 0 ? _userModel.SelectedBoardId : _items.First().DataId;
            SelectBoard(boardId);

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

            var boardId = _configStorage.CustomizationDataList
                    .FirstOrDefault(data => data.Id == _customizationModel.SelectedBoardId.Value)
                    .PrefabId;
            _userModel.ProcessBoardSelection(boardId);
        }

        private void CreateItems()
        {
            var dataList = _customizationModel.BoardDataList;
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
            SelectBoard(id);
        }

        private void OnConfirmUnlock(int id)
        {
            _userModel.UnlockCustomizationItem(id);
            _items.FirstOrDefault(i => i.DataId == id).SetUnlocked();
            OnItemClick(id);
        }

        private void SelectBoard(int id)
        {
            _customizationModel.SelectBoard(id);
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
