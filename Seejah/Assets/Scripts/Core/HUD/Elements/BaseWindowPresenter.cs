using Assets.Scripts.Core.Presenters;
using System;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Core.HUD.Elements
{
    public class BaseWindowPresenter : MonoBehPresenter
    {
        [SerializeField] private TextMeshProUGUI textTitle;
        [SerializeField] private Button buttonClose;
        [SerializeField] private Image interactiveArea;

        protected string Title
        {
            set => textTitle.text = value;
        }

        protected Action CloseAction
        {
            set
            {
                AddForDispose(buttonClose
                    .OnClickAsObservable()
                    .Subscribe(_ => value.Invoke()));
                AddForDispose(interactiveArea
                    .OnPointerClickAsObservable()
                    .Subscribe(_ => value.Invoke()));
            }
        }

        protected bool IsLandscape => Screen.height / Screen.width < 1;
    }
}
