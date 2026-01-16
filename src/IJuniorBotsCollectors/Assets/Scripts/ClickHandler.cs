using System.Collections;
using Base;
using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Flag _flagPrefab;
    
    private Flag _flag;
    private Coroutine _moveFlagCoroutine;
    private Vector2 _mousePosition;
    private ResourceBase _resourceBase;
    
    private void OnEnable()
    {
        _inputReader.Clicked += OnClicked;
        _inputReader.MousePositionChanged += SetMousePosition;
    }

    private void OnDisable()
    {
        _inputReader.Clicked -= OnClicked;
        _inputReader.MousePositionChanged -= SetMousePosition;
    }

    private void OnClicked(Vector2 mousePosition)
    {
        Ray ray = _mainCamera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (_flag == null && hit.collider.TryGetComponent(out _resourceBase))
            {
                if (_resourceBase.TryTakeFlag())
                {
                    _flag = Instantiate(_flagPrefab);
                    _flag.Construct(_resourceBase);

                    StopMoveFlagCoroutine();
                    _moveFlagCoroutine = StartCoroutine(MoveFlag());
                }
            }
            else
            {
                StopMoveFlagCoroutine();

                _resourceBase.ChangePriorityToBuild(_flag.transform.position);
                
                _resourceBase = null;
                _flag = null;
            }
        } 
    }

    private void SetMousePosition(Vector2 mousePosition) => 
        _mousePosition = mousePosition;

    private IEnumerator MoveFlag()
    {
        while (_flag)
        {
            Ray ray = _mainCamera.ScreenPointToRay(_mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.TryGetComponent(out Flag thisFlag) == false) 
                _flag.transform.position = hit.point;

            yield return null;
        }
    }

    private void StopMoveFlagCoroutine()
    {
        if (_moveFlagCoroutine != null)
        {
            StopCoroutine(_moveFlagCoroutine);
            _moveFlagCoroutine = null;
        }
    }
}