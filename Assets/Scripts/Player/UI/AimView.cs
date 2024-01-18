using System;
using UnityEngine;

namespace Player.UI
{
    public class AimView : MonoBehaviour
    {
        //TODO: Rewrite this
        [SerializeField] private float lineWidth;

        public float _thetaScale = 0.01f;
        private float _theta;
        private float _sightRadius = 3f;
        
        private int _size;
        private LineRenderer _lineDrawer;
        
        public bool IsEnabled { get; set; }

        private void Start()
        {
            _lineDrawer = GetComponent<LineRenderer>();
        }

        [Obsolete("Obsolete")]
        private void LateUpdate()
        {
            UpdateView();
        }

        [Obsolete("Obsolete")]
        private void UpdateView()
        {
            _theta = 0f;
            _size = (int)((1f / _thetaScale) + 1f);
            _lineDrawer.SetVertexCount(_size);
            
            for (int i = 0; i < _size; i++)
            {
                _theta += (2.0f * Mathf.PI * _thetaScale);
                float x = _sightRadius * Mathf.Cos(_theta);
                float y = _sightRadius * Mathf.Sin(_theta);
                _lineDrawer.SetPosition(i, new Vector3(x, y, 0));
            }
        }
    }
}