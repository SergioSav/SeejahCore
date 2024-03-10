using Assets.Scripts.Core.Presenters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using System;

namespace Assets.Scripts.Core.HUD.Gameplay.Tutorial
{
    public abstract class TutorialWindowAbstractContent : MonoBehPresenter, ITutorialWindowContent
    {
        [SerializeField] private Image imageTutorial;
        [SerializeField] private TextMeshProUGUI textTutorial;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button leftArrowButton;
        [SerializeField] private Button rightArrowButton;
        [SerializeField] private Button buttonClose;
        [SerializeField] private Transform infoDotPlace;

        public Transform DotsPlace => infoDotPlace;
        public Button LeftArrow => leftArrowButton;
        public Button NextButton => nextButton;
        public Button RightArrow => rightArrowButton;

        public void Setup(Action OnRightClick, Action OnLeftClick, Action OnNextClick, Action onCloseClick)
        {
            AddForDispose(NextButton.OnPointerClickAsObservable().Subscribe(_ => OnNextClick()));
            AddForDispose(LeftArrow.OnPointerClickAsObservable().Subscribe(_ => OnLeftClick()));
            AddForDispose(RightArrow.OnPointerClickAsObservable().Subscribe(_ => OnRightClick()));
            AddForDispose(buttonClose.OnPointerClickAsObservable().Subscribe(_ => onCloseClick()));

        }

        public void SetImage(Sprite sprite)
        {
            imageTutorial.sprite = sprite;
        }

        public void SetText(string text)
        {
            textTutorial.text = text;
        }

        public void UpdateVisibility(bool needShow)
        {
            gameObject.SetActive(needShow);
        }

    }
}