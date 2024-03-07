using UnityEngine;

namespace Assets.Scripts.Core.HUD
{
    public class CameraController : MonoBehaviour
    {
        private Camera _camera;
        private float _startFOV;

        private void Start()
        {
            _camera = Camera.main;
            _startFOV = _camera.fieldOfView;
        }

        private void Update()
        {
            var coef = (float)Screen.height / Screen.width;
            _camera.fieldOfView = _startFOV * Mathf.Max(coef, 1f);
        }
    }
}
