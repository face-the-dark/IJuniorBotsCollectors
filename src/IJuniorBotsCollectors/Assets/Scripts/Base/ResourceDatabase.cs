using System.Collections.Generic;
using System.Linq;
using Base.Scanner;
using ResourceComponents;
using UnityEngine;

namespace Base
{
    public class ResourceDatabase : MonoBehaviour
    {
        [SerializeField] private ResourceScanner _resourceScanner;
        
        private List<Resource> _foundResources;
        private List<Resource> _busyResources;
        
        public int FoundResourcesCount => _foundResources.Count;

        private void Awake()
        {
            _foundResources = new List<Resource>();
            _busyResources = new List<Resource>();
        }

        private void OnDisable() => 
            _resourceScanner.ResourcesFound -= AddFoundResources;

        private void OnEnable() => 
            _resourceScanner.ResourcesFound += AddFoundResources;

        public Resource GetFreeResource()
        {
            if (_foundResources.Count <= 0)
                return null;

            Resource resource = _foundResources.First();
            
            _foundResources.Remove(resource);
            _busyResources.Add(resource);
            
            return resource;
        }

        public void Release(Resource resource) => 
            _busyResources.Remove(resource);

        private void AddFoundResources(List<Resource> resources)
        {
            foreach (Resource resource in resources)
                if (_foundResources.Contains(resource) == false && _busyResources.Contains(resource) == false)
                    _foundResources.Add(resource);
        }
    }
}