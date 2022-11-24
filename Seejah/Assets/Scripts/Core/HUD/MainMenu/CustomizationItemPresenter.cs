using Assets.Scripts.Core.Commands;
using Assets.Scripts.Core.Data;
using Assets.Scripts.Core.Data.Services;
using Assets.Scripts.Core.Presenters;
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
        private ISpriteSupplier _spriteSupplier;
        private ISelectCustomizationItemCommand _selectCommand;
        private bool _isSelected;

        [Inject]

        public void Construct(ISpriteSupplier spriteSupplier, ISelectCustomizationItemCommand selectCommand)
        {
            _spriteSupplier = spriteSupplier;
            _selectCommand = selectCommand;
        }

        public void SetData(CustomizationData data)
        {
            _data = data;
        }

        public void Start()
        {
            textTitle.text = _data.Name;
            imageIcon.sprite = _spriteSupplier.GetSprite(_data.ImageId);
            UpdateSelectionFrame();

            AddForDispose(selectionArea.OnPointerClickAsObservable()
                .Subscribe(_ => _selectCommand.Execute(_data.Id)));
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
