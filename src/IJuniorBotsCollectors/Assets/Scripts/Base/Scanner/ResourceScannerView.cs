using System.Collections;
using UnityEngine;

namespace Base.Scanner
{
    public class ResourceScannerView : MonoBehaviour
    {
        private const float RadiusModifier = 2f;
        
        [SerializeField] private float _growTime = 1f;
        [SerializeField] private ResourceScanner _scanner;
        [SerializeField] private GameObject _scanEffectPrefab;

        private Coroutine _growCoroutine;
        private float _growTimeProgress;
        private GameObject _scanEffect;

        private void Awake()
        {
            _scanEffect = Instantiate(_scanEffectPrefab, transform.position, Quaternion.identity);
            _scanEffect.SetActive(false);
        }

        private void OnEnable() => 
            _scanner.ScanStarting += OnScanStarting;

        private void OnDisable() => 
            _scanner.ScanStarting -= OnScanStarting;

        private void OnScanStarting()
        {
            StopGrowCoroutine();
            _growCoroutine = StartCoroutine(Grow());
        }

        private void StopGrowCoroutine()
        {
            if (_growCoroutine != null)
            {
                StopCoroutine(_growCoroutine);
                _growCoroutine = null;
            }
        }

        private IEnumerator Grow()
        {
            ResetScanEffect();
            
            _growTimeProgress = 0f;

            while (_growTimeProgress < _growTime)
            {
                _growTimeProgress += Time.deltaTime;

                float growStep = Mathf.Clamp01(_growTimeProgress / _growTime);
                float currentSize = growStep * _scanner.ScanRadius * RadiusModifier;
                _scanEffect.transform.localScale = Vector3.one * currentSize;

                yield return null;
            }
            
            _scanEffect.SetActive(false);
        }

        private void ResetScanEffect()
        {
            _scanEffect.transform.localScale = Vector3.zero;
            _scanEffect.SetActive(true);
        }
    }
}