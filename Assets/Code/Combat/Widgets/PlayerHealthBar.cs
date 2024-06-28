using System;
using UnityEngine;

namespace TN141.Combat.Widgets
{
    public class PlayerHealthBar : MonoBehaviour
    {
        private Camera _cam;
        
        private void Awake()
        {
            _cam = Camera.main;
        }

        private void Update()
        {
            transform.LookAt(
                transform.position + _cam.transform.rotation * Vector3.forward,
                _cam.transform.rotation * Vector3.up);
            // transform.LookAt(_cam.transform);
        }
    }
}