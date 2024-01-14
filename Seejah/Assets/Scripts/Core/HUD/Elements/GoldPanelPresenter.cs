using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.Presenters;
using TMPro;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.Core.HUD.Elements
{
    public class GoldPanelPresenter : MonoBehPresenter
    {
        [SerializeField] private TextMeshProUGUI textAmount;

        public void Setup(UserModel userModel)
        {
            AddForDispose(userModel.CurrentGold.Subscribe(v => UpdateAmount(v)));
        }

        private void UpdateAmount(int amount)
        {
            textAmount.text = amount.ToString();
        }
    }
}
