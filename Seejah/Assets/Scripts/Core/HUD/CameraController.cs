using UnityEngine;

namespace Assets.Scripts.Core.HUD
{
    public class CameraController : MonoBehaviour
    {
        private Camera _camera;
        private float _startFOV;

        private void Start()
        {
            _camera = GetComponent<Camera>();
            _startFOV = _camera.fieldOfView;
        }

        private void Update()
        {
            float coef = (float)Screen.height / Screen.width;
            _camera.fieldOfView = _startFOV * Mathf.Min(coef, 1f);
        }
    }
}
