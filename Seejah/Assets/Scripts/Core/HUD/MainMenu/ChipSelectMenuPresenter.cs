using Assets.Scripts.Core.Data;
using Assets.Scripts.Core.HUD.Elements;
using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.SceneInstallers;
using Assets.Scripts.Core.Utils;
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
    public class ChipSelectMenuPresenter : BaseWindowPresenter
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
        private ShopService _shopService;
        private ConfirmUnlockWindowPresenter _unlockWindowPresenter;
        private AudioService _audioService;
        private Func<CustomizationItemPresenter, Transform, CustomizationItemPresenter> _itemFactory;
        private readonly List<CustomizationItemPresenter> _items = new List<CustomizationItemPresenter>();

        [Inject]
        public void Construct(UserModel userModel, CustomizationModel customizationModel, ShopService shopService,
                ConfirmUnlockWindowPresenter unlockWindowPresenter, AudioService audioService,
                Func<CustomizationItemPresenter, Transform, CustomizationItemPresenter> itemFactory)
        {
            _userModel = userModel;
            _customizationModel = customizationModel;
            _shopService = shopService;
            _unlockWindowPresenter = unlockWindowPresenter;
            _audioService = audioService;
            _itemFactory = itemFactory;
        }

        private void OnStateChange(CustomizationState state)
        {
            gameObject.SetActive(state == CustomizationState.ChipCustomization);
        }

        private void Start()
        {
            Title = "Chips";
            CloseAction = OnMenuClose;

            CreateItems();

            AddForDispose(_customizationModel.CurrentState.Subscribe(OnStateChange));
            AddForDispose(_customizationModel.SelectedChipId.Subscribe(OnItemSelect));
            AddForDispose(confirmButton.OnPointerClickAsObservable().Subscribe(_ => OnMenuClose()));
            AddForDispose(leftArrowButton.OnPointerClickAsObservable().Subscribe(_ => OnLeftClick()));
            AddForDispose(rightArrowButton.OnPointerClickAsObservable().Subscribe(_ => OnRightClick()));

            goldPanel.Setup(_userModel);
        }

        private void OnEnable()
        {
            var chipId = _userModel.SelectedChipId != 0 ? _userModel.SelectedChipId : _items.First().DataId;
            SelectChip(chipId);
        }

        private void OnRightClick()
        {
            _audioService.PlayUISound(SoundType.Click);
            itemsParent.transform.localPosition += Vector3.left * _SLIDER_MOVE_X;
        }

        private void OnLeftClick()
        {
            _audioService.PlayUISound(SoundType.Click);
            itemsParent.transform.localPosition += Vector3.right * _SLIDER_MOVE_X;
        }

        private void OnMenuClose()
        {
            _audioService.PlayUISound(SoundType.Click);
            _customizationModel.EndConcreteCustomization();

            _userModel.ProcessChipSelection(_customizationModel.SelectedChipId.Value);
        }

        private void CreateItems()
        {
            var chipDataList = _customizationModel.ChipDataList;
            for (int i = 0; i < chipDataList.Count; i++)
            {
                var data = chipDataList[i];
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
                var item = _customizationModel.FullDataList.FirstOrDefault(m => m.Id == id);
                if (_shopService.CanBuyFor(item.Price))
                    _unlockWindowPresenter.ShowConfirm(item, OnConfirmUnlock);
                return;
            }
            SelectChip(id);
        }

        private void OnConfirmUnlock(CustomizationData data)
        {
            _shopService.ProcessBuyFor(data.Price);
            _userModel.UnlockCustomizationItem(data.Id);
            _items.FirstOrDefault(i => i.DataId == data.Id).SetUnlocked();
            OnItemClick(data.Id);
        }

        private void SelectChip(int id)
        {
            _customizationModel.SelectChip(id);
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
