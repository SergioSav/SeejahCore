using Assets.Scripts.Core.Data;
using Assets.Scripts.Core.Data.Services;
using Assets.Scripts.Core.Presenters;
using System;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Assets.Scripts.Core.HUD
{
    public class CustomizationItemPresenter : MonoBehPresenter
    {
        [SerializeField] private TextMeshProUGUI textTitle;
        [SerializeField] private Image imageIcon;
        [SerializeField] private Image selectionArea;
        [SerializeField] private GameObject selectionFrame;

        private CustomizationData _data;
        private Action<int> _onSelect;
        private ISpriteSupplier _spriteSupplier;
        private bool _isSelected;

        [Inject]
        public void Construct(ISpriteSupplier spriteSupplier)
        {
            _spriteSupplier = spriteSupplier;
        }

        public void Setup(CustomizationData data, Action<int> onSelect)
        {
            _data = data;
            _onSelect = onSelect;
        }

        public void Start()
        {
            textTitle.text = _data.Name;
            imageIcon.sprite = _spriteSupplier.GetSprite(_data.ImageId);
            UpdateSelectionFrame();

            AddForDispose(selectionArea.OnPointerClickAsObservable()
                .Subscribe(_ => _onSelect?.Invoke(_data.Id)));
        }

        public void SetSelected(int id)
        {
            _isSelected = _data.Id == id;
            UpdateSelectionFrame();
        }

        private void UpdateSelectionFrame()
        {
            selectionFrame.SetActive(_isSelected);
        }
    }
}
