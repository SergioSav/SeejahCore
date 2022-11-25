using UnityEngine;

namespace Assets.Scripts.Core.Views
{
    public class SelectionCellView : MonoBehaviour
    {

        public void ShowFor(CellView cellView)
        {
            gameObject.SetActive(true);
            transform.localPosition = cellView.transform.localPosition;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}