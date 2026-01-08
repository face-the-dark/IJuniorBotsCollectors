using System;
using System.Collections;
using System.Collections.Generic;
using ResourceComponents;
using UnityEngine;

namespace Base.Scanner
{
    public class ResourceScanner : MonoBehaviour
    {
        private const int ScanResultsSize = 100;
        
        [SerializeField] private float _scanRadius = 70f;
        [SerializeField] private float _scanDelay = 4f;
        
        private Coroutine _scanCoroutine;
        private WaitForSeconds _wait;
        private Collider[] _scanResults;

        public event Action ScanStarting;
        public event Action<List<Resource>> ResourcesFound;
        
        public float ScanRadius => _scanRadius;
        
        private void Awake()
        {
            _wait = new WaitForSeconds(_scanDelay);
            _scanResults = new Collider[ScanResultsSize];
        }

        private void Start() => 
            ScanForResources();

        private void ScanForResources()
        {
            StopScanCoroutine();
            _scanCoroutine = StartCoroutine(Scan());
        }

        private void StopScanCoroutine()
        {
            if (_scanCoroutine != null)
            {
                StopCoroutine(_scanCoroutine);
                _scanCoroutine = null;
            }
        }

        private IEnumerator Scan()
        {
            while (enabled)
            {
                ScanStarting?.Invoke();
                
                Find();

                yield return _wait;
            }
        }

        private void Find()
        {
            int collidersCount = Physics.OverlapSphereNonAlloc(transform.position, _scanRadius, _scanResults);

            List<Resource> foundResources = new List<Resource>();

            for (int i = 0; i < collidersCount; i++)
            {
                Resource resource = _scanResults[i].GetComponent<Resource>();
                    
                if (resource) 
                    foundResources.Add(resource);
            }
                
            ResourcesFound?.Invoke(foundResources);
        }
    }
}