using Assets.Scripts.Core.Presenters;
using System;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Core.HUD.Elements
{
    public class BaseComboBoxPresenter : MonoBehPresenter
    {
        [SerializeField] private Image interactiveArea;
        [SerializeField] private GameObject dropdownMenu;
        [SerializeField] private Transform itemsPlace;
        [SerializeField] private BaseComboBoxItemPresenter itemPrototype;
        [SerializeField] private TextMeshProUGUI textSelectedVariant;

        private List<BaseComboBoxItemPresenter> _items;
        private bool _isDropdownMenuOpen;

        private Dictionary<string, string> _variants;
        private Action<string> _onVariantClick;
        private string _currentId;

        public void Setup(Dictionary<string, string> variants, Action<string> onVariantClick)
        {
            _variants = variants;
            _onVariantClick = onVariantClick;
            CreateItems();
        }

        public void SelectVariant(string id)
        {
            _currentId = id;
            textSelectedVariant.text = _variants[id];
            _onVariantClick?.Invoke(id);
            UpdateVariantList();
        }

        public void TryCloseDropdownMenu()
        {
            if (!_isDropdownMenuOpen)
                return;
            _isDropdownMenuOpen = false;
            UpdateView();
        }

        private void Start()
        {
            _isDropdownMenuOpen = false;
            AddForDispose(interactiveArea.OnPointerClickAsObservable().Subscribe(_ => SwitchDropdownMenuState()));
        }

        private void SwitchDropdownMenuState()
        {
            _isDropdownMenuOpen = !_isDropdownMenuOpen;
            UpdateView();
        }

        private void UpdateView()
        {
            dropdownMenu.SetActive(_isDropdownMenuOpen);
        }

        private void UpdateVariantList()
        {
            var counter = 0;
            foreach (var kvp in _variants)
            {
                if (kvp.Key == _currentId)
                    continue;
                _items[counter++].Setup(kvp.Key, kvp.Value, OnVariantClick);
            }
        }

        private void OnVariantClick(string id)
        {
            SelectVariant(id);
            SwitchDropdownMenuState();
        }

        private void CreateItems()
        {
            _items = new List<BaseComboBoxItemPresenter>();
            for (int i = 0; i < _variants.Count - 1; i++)
            {
                var item = CreateItem();
                _items.Add(item);
            }
        }

        private BaseComboBoxItemPresenter CreateItem()
        {
            return Instantiate(itemPrototype, itemsPlace);
        }

        private void OnDisable()
        {
            TryCloseDropdownMenu();
        }

        private void Update()
        {
            if (Input.touchSupported)
            {
                if (Input.touchCount > 0 && !EventSystem.current.IsPointerOverGameObject())
                    TryCloseDropdownMenu();
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject())
                    TryCloseDropdownMenu();
            }
        }
    }
}