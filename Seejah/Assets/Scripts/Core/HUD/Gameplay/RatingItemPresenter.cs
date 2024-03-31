using Assets.Scripts.Core.Presenters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Core.HUD
{
    public class RatingItemPresenter : MonoBehPresenter
    {
        [SerializeField] private TextMeshProUGUI textAmount;
        [SerializeField] private Image imageRatingUp;
        [SerializeField] private Image imageRatingDown;

        //[Inject]
        //public void Construct()
        //{
        //}

        public void Setup(int ratingChangeValue)
        {
            var signStr = ratingChangeValue > 0 ? "+" : "-";
            textAmount.text = $"{signStr}{ratingChangeValue}";
            imageRatingUp.gameObject.SetActive(ratingChangeValue > 0);
            imageRatingDown.gameObject.SetActive(ratingChangeValue <= 0);
        }
    }
}
