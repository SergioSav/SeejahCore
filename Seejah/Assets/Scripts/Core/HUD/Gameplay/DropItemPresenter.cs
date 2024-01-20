using Assets.Scripts.Core.Data;
using Assets.Scripts.Core.Presenters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Core.HUD
{
    public class DropItemPresenter : MonoBehPresenter
    {
        [SerializeField] private TextMeshProUGUI textAmount;
        [SerializeField] private Image imageIcon;

        private DropData _data;

        //[Inject]
        //public void Construct()
        //{
        //}

        public void Setup(DropData dropData)
        {
            _data = dropData;
            textAmount.text = _data.Value.ToString();
        }
    }
}
