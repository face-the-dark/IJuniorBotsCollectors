using System;
using System.Collections;
using ResourceComponents;
using UnityEngine;

namespace UnitComponents
{
    [RequireComponent(typeof(ResourceDetector))]
    public class ResourceCollector : MonoBehaviour
    {
        [SerializeField] private Transform _attachPoint;
        [SerializeField] private float _collectingTime = 1f;

        private Coroutine _collectCoroutine;

        public event Action Collected;
    
        public void Collect(Resource resource)
        {
            StopCollectCoroutine();
            StartCoroutine(Reparent(resource));
        }

        private void StopCollectCoroutine()
        {
            if (_collectCoroutine != null)
            {
                StopCoroutine(_collectCoroutine);
                _collectCoroutine = null;
            }
        }

        private IEnumerator Reparent(Resource resource)
        {
            yield return new WaitForSeconds(_collectingTime);
        
            resource.transform.SetParent(_attachPoint);
            resource.transform.position = _attachPoint.position;
        
            Collected?.Invoke();
        }
    }
}