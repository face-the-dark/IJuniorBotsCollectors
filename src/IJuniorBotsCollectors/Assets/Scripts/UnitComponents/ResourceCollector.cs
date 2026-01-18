using System;
using System.Collections;
using ResourceComponents;
using UnityEngine;

namespace UnitComponents
{
    [RequireComponent(typeof(UnitMover))]
    public class ResourceCollector : MonoBehaviour
    {
        [SerializeField] private Transform _attachPoint;
        [SerializeField] private float _collectingTime = 1f;
        
        private Coroutine _collectCoroutine;
        private WaitForSeconds _wait;
        
        public event Action Collected;

        public bool IsCollected { get; private set; }

        private void Awake() => 
            _wait = new WaitForSeconds(_collectingTime);

        public void Collect(Resource resource)
        {
            StopCollectCoroutine();
            _collectCoroutine = StartCoroutine(Reparent(resource));
        }

        public void Reset() => 
            IsCollected = false;

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
            yield return _wait;
        
            resource.transform.SetParent(_attachPoint);
            resource.transform.position = _attachPoint.position;
        
            IsCollected = true;
            
            Collected?.Invoke();
        }
    }
}