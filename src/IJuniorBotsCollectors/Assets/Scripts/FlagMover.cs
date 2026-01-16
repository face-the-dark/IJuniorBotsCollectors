using System.Collections;
using Base;
using UnityEngine;

public class FlagMover : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private LayerMask _groundLayer;

    private Coroutine _moveFlagCoroutine;
    private Vector2 _mousePosition;
    private Flag _takenFlag;
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

        if (Physics.Raycast(ray, out RaycastHit hit, _groundLayer))
        {
            if (_takenFlag == null && hit.collider.TryGetComponent(out _resourceBase))
            {
                _takenFlag = _resourceBase.TakeFlag();
                
                StopMoveFlagCoroutine();
                _moveFlagCoroutine = StartCoroutine(MoveFlag());
            }
            else if (_takenFlag)
            {
                StopMoveFlagCoroutine();

                _resourceBase.ChangePriority(TargetPriority.BuildNewResourceBase);

                Reset();
            }
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

    private IEnumerator MoveFlag()
    {
        while (_takenFlag)
        {
            Ray ray = _mainCamera.ScreenPointToRay(_mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.TryGetComponent(out Flag thisFlag) == false)
                _takenFlag.transform.position = hit.point;

            yield return null;
        }
    }

    private void Reset()
    {
        _resourceBase = null;
        _takenFlag = null;
    }

    private void SetMousePosition(Vector2 mousePosition) =>
        _mousePosition = mousePosition;
}