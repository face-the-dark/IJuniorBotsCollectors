using UnityEngine;

public class Resource : MonoBehaviour
{
    public void Reset() => 
        transform.SetParent(null);
}