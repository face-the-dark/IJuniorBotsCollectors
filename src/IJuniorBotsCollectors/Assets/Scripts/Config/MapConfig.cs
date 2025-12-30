using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "NewMapConfig", menuName = "Config/MapConfig")]
    public class MapConfig : ScriptableObject
    {
        [SerializeField] private float _minEdgeSize = -49f;
        [SerializeField] private float _maxEdgeSize = 49f;
    
        public float MinEdgeSize => _minEdgeSize;
        public float MaxEdgeSize => _maxEdgeSize;
    }
}