using Assets.Scripts.Core.Presenters;
using System;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Core.HUD.Elements
{
    public class BaseComboBoxItemPresenter : MonoBehPresenter
    {
        [SerializeField] private Image interactiveArea;
        [SerializeField] private TextMeshProUGUI textVariant;

        private Action<string> _onClick;

        public string Id { get; private set; }

        public void Setup(string id, string text, Action<string> onClick)
        {
            Id = id;
            _onClick = onClick;
            textVariant.text = text;
        }

        private void Start()
        {
            AddForDispose(interactiveArea.OnPointerClickAsObservable().Subscribe(_ => _onClick?.Invoke(Id)));
        }
    }
}