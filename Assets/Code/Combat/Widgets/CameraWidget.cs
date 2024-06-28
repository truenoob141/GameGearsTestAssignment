using TN141.Configs;
using UnityEngine;
using Zenject;

namespace TN141.Combat.Widgets
{
    [RequireComponent(typeof(Camera))]
    public class CameraWidget : MonoBehaviour
    {
        [Inject]
        private readonly ConfigService _configService;

        [SerializeField]
        private Transform _centerAnchorTransform;

        private Camera _camera;
        private bool _init;
        private Vector3 _startPos;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        private void Update()
        {
            var settings = _configService.GetCameraSettings();
            if (settings == null)
                return;

            // Initialize
            if (!_init)
            {
                _init = true;
                
                var pos = _camera.transform.position;
                pos.y = settings.height;
                _camera.transform.position = pos;
                _startPos = _camera.transform.position;
            }

            // Update fov
            UpdateFov(settings);
            
            // Calculate position
            float progress = (Time.time % settings.roundDuration) / settings.roundDuration;

            var center = _centerAnchorTransform.position;

            var trans = transform;
            var dir = _startPos - center;
            dir = Quaternion.Euler(Vector3.up * (progress * 360f)) * dir;
            trans.position = center + dir;
            
            // Rotate. Look at center
            trans.LookAt(center + Vector3.up * settings.lookAtHeight);
            
            // Roaming
            float angle = Time.time * Mathf.PI / settings.roamingDuration;
            trans.position += trans.up * (settings.roamingRadius * Mathf.Sin(angle));
        }

        private void UpdateFov(CameraModel settings)
        {
            float delay = settings.fovDelay;
            float animationDuration = settings.fovDuration;

            float totalTime = delay + animationDuration;
            // Looping
            float t = Time.time % (totalTime * 2.0f); 

            float value;
            // To top
            if (t < animationDuration)
                value = t / animationDuration; 
            // Top Delay
            else if (t < totalTime)
                value = 1;
            // To bot
            else if (t < totalTime + animationDuration)
                value = 1 - (t - totalTime) / animationDuration;
            // Bot delay
            else
                value = 0;

            float fov = Mathf.Lerp(settings.fovMin, settings.fovMax, value);
            _camera.fieldOfView = fov;
        }
    }
}