using Assets.Scripts.Core.Data;
using Assets.Scripts.Core.Data.Services;
using Assets.Scripts.Core.Presenters;
using Assets.Scripts.Core.Utils.AudioService;
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
        [SerializeField] private GameObject lockBlock;
        [SerializeField] private TextMeshProUGUI textPrice;

        private CustomizationData _data;
        private bool _isLocked;
        private Action<int> _onSelect;
        private ISpriteSupplier _spriteSupplier;
        private AudioService _audioService;
        private bool _isSelected;

        [Inject]
        public void Construct(ISpriteSupplier spriteSupplier, AudioService audioService)
        {
            _spriteSupplier = spriteSupplier;
            _audioService = audioService;
        }

        public void Setup(CustomizationData data, bool isLocked, Action<int> onSelect)
        {
            _data = data;
            _isLocked = isLocked;
            _onSelect = onSelect;
        }

        public void Start()
        {
            textTitle.text = _data.Name;
            imageIcon.sprite = _spriteSupplier.GetSprite(_data.ImageId);
            UpdateSelectionFrame();
            UpdateLockState();

            AddForDispose(selectionArea.OnPointerClickAsObservable()
                .Subscribe(_ => OnSelect()));
        }

        public void SetSelected(int id)
        {
            _isSelected = _data.Id == id;
            UpdateSelectionFrame();
        }

        public void SetUnlocked()
        {
            _isLocked = false;
            UpdateLockState();
        }

        public int DataId => _data.Id;


        private void OnSelect()
        {
            _audioService.PlayUISound(SoundType.Click);
            _onSelect?.Invoke(_data.Id);
        }

        private void UpdateSelectionFrame()
        {
            selectionFrame.SetActive(_isSelected);
        }

        private void UpdateLockState()
        {
            lockBlock.SetActive(_isLocked);
            textPrice.text = _data.Price.Value.ToString();
        }
    }
}
