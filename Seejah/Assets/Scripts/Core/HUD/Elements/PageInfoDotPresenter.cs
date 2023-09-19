using UnityEngine;

namespace Assets.Scripts.Core.HUD.Elements
{
    public class PageInfoDotPresenter : MonoBehaviour
    {
        [SerializeField] private GameObject imageNormal;
        [SerializeField] private GameObject imageActive;

        public void SwitchActive(bool isActive)
        {
            imageActive.SetActive(isActive);
            imageNormal.SetActive(!isActive);
        }
    }
}
