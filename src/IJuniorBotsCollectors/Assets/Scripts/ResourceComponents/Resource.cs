using UnityEngine;

namespace ResourceComponents
{
    public class Resource : MonoBehaviour
    {
        public void Init() => 
            gameObject.SetActive(true);

        public void Reset()
        {
            gameObject.SetActive(false);
            transform.SetParent(null);
        }
    }
}