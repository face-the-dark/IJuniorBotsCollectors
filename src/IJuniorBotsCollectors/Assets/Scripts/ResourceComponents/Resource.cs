using UnityEngine;

namespace ResourceComponents
{
    public class Resource : MonoBehaviour
    {
        public bool IsBusy { get; set; }

        public void Init() => 
            gameObject.SetActive(true);

        public void Reset()
        {
            gameObject.SetActive(false);
            transform.SetParent(null);
        }
    }
}