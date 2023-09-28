using System.Collections;
using Player.Effects;
using UnityEngine;
using UnityEngine.UI;

namespace Effects
{
    public class FlashEffect : Effect
    {
        private GameObject _effectObject;
        private Canvas _blindnessCanvas;
        private Image _blindnessImage;

        private float _initialAlpha = 1f;
        private float _targetAlpha = 0f;

        private float _duration;
    
        public void Initialize(float duration)
        {
            _duration = duration;

            _effectObject = new GameObject("BlindnessEffect");
            _effectObject.transform.SetParent(transform, false);
            
            _blindnessCanvas = _effectObject.AddComponent<Canvas>();
            _blindnessCanvas.renderMode = RenderMode.ScreenSpaceOverlay;

            _blindnessImage = _blindnessCanvas.gameObject.AddComponent<Image>();
            _blindnessImage.color = Color.white;

            RectTransform rectTransform = _blindnessImage.GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.sizeDelta = Vector2.zero;

            _blindnessCanvas.enabled = false;
        }
    
        protected override void PerformEffect()
        {
            StartCoroutine(PerformEffectRoutine());
        }

        private void Start()
        {
            PerformEffect();
        }
    
        private IEnumerator PerformEffectRoutine()
        {
            if (_blindnessImage != null)
            {
                _blindnessCanvas.enabled = true;

                for (int i = 0; i <= 100; i++)
                {
                    float alpha = Mathf.Lerp(_initialAlpha, _targetAlpha, Mathf.Pow(i / 100f, _duration));
                    Color currentColor = _blindnessImage.color;
                    currentColor.a = alpha;
                    _blindnessImage.color = currentColor;

                    yield return new WaitForSeconds(_duration / 100f);
                }
                
                Destroy(this);
            }
        }

        private void OnDestroy()
        {
            Destroy(_effectObject);
        }
    }
}