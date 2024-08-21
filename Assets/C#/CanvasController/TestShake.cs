using System;
using UnityEngine;

namespace C_.CanvasController
{
    public class TestShake : MonoBehaviour
    {
        public float amplitude = 5f;//抖动幅度
        public float frequency = 25f;//抖动频率

        private RectTransform _rectTransform;
        private Vector3 _originalPosition;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _originalPosition = _rectTransform.anchoredPosition;
        }

        private void Update()
        {
            float offsetX = Mathf.PerlinNoise(Time.time * frequency, 0f) * amplitude - amplitude / 2;
            float offsetY = Mathf.PerlinNoise(0f , Time.time * frequency) * amplitude - amplitude / 2;
            _rectTransform.anchoredPosition = _originalPosition + new Vector3(offsetX, offsetY, 0);
        }
    }
}
