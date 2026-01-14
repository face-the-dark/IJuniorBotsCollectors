using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;

    private void OnEnable() => 
        _inputReader.Clicked += OnClicked;

    private void OnDisable() => 
        _inputReader.Clicked -= OnClicked;

    private void OnClicked(Vector2 mousePosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit)) 
            Debug.Log(hit.collider.gameObject.name);
    }
}