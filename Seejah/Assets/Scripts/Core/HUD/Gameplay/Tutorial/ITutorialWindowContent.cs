using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Core.HUD.Gameplay.Tutorial
{
    public interface ITutorialWindowContent
    {
        void UpdateVisibility(bool needShow);
        void SetImage(Sprite sprite);
        void SetText(string text);

        Button LeftArrow { get; }
        Button RightArrow { get; }
        Button NextButton { get; }
        Transform DotsPlace { get; }
    }
}