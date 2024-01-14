using Assets.Scripts.Core.Data;
using Assets.Scripts.Core.HUD.Elements;
using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Assets.Scripts.Core.HUD
{
    public class ConfirmUnlockWindowPresenter : BaseWindowPresenter
    {
        [SerializeField] private TextMeshProUGUI textMessage;
        [SerializeField] private Button buttonConfirm;
        [SerializeField] private Button buttonCancel;

        private CustomizationData _data;
        private Action<int> _onConfirm;
        private Action _onClose;

        //[Inject]
        //public void Construct()
        //{
        //}

        public int DataId => _data.Id;

        public void ShowConfirm(CustomizationData data, Action<int> onConfirm, Action onClose = null)
        {
            _data = data;
            _onConfirm = onConfirm;
            _onClose = onClose;
            gameObject.SetActive(true);
        }

        private void Start()
        {
            Title = "Confirmation";
            CloseAction = OnClose;

            AddForDispose(buttonConfirm.OnClickAsObservable()
                .Subscribe(_ => OnConfirm()));
            AddForDispose(buttonCancel.OnClickAsObservable()
                .Subscribe(_ => OnClose()));

            CloseWindow();
        }

        private void OnConfirm()
        {
            _onConfirm?.Invoke(_data.Id);
            CloseWindow();
        }

        private void OnClose()
        {
            _onClose?.Invoke();
            CloseWindow();
        }

        private void CloseWindow()
        {
            gameObject.SetActive(false);
        }
    }
}
