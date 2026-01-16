using System.Collections;
using UnityEngine;

namespace Base.Scanner
{
    [RequireComponent(typeof(ResourceScanner))]
    public class ResourceScannerView : MonoBehaviour
    {
        private const float RadiusModifier = 2f;
        private const float TransparencyModifier = 4;

        [SerializeField] private float _growTime = 0.5f;
        [SerializeField] private GameObject _scanEffectPrefab;

        private ResourceScanner _scanner;

        private Coroutine _growCoroutine;
        private float _growTimeProgress;
        private GameObject _scanEffect;
        private Renderer _baseScanEffectRenderer;
        private Material _baseScanEffectMaterial;

        private void Awake()
        {
            _scanner = GetComponent<ResourceScanner>();

            _scanEffect = Instantiate(_scanEffectPrefab, transform.position, Quaternion.identity);
            _scanEffect.SetActive(false);
            _baseScanEffectRenderer = _scanEffect.GetComponent<Renderer>();
            _baseScanEffectMaterial = _baseScanEffectRenderer.material;
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
                
                float materialAlpha = 1 / TransparencyModifier - growStep / TransparencyModifier;
                
                _baseScanEffectRenderer.material.color = new Color
                (
                    _baseScanEffectMaterial.color.r,
                    _baseScanEffectMaterial.color.g,
                    _baseScanEffectMaterial.color.b,
                    materialAlpha
                );

                yield return null;
            }

            _scanEffect.SetActive(false);
        }

        private void ResetScanEffect()
        {
            _scanEffect.transform.localScale = Vector3.zero;
            _scanEffect.SetActive(true);
            _baseScanEffectRenderer.material = _baseScanEffectMaterial;
        }
    }
}