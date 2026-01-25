using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    private PlayerInput _playerInput;
    
    private Vector2 _lastMousePosition;
    
    public event Action<Vector3> Moved;
    public event Action<Vector2> Clicked;
    public event Action<Vector2> MousePositionChanged;
    
    private void Awake() => 
        _playerInput = new PlayerInput();

    private void OnEnable()
    {
        _playerInput.Enable();
        
        _playerInput.Player.Move.performed += OnMove;
        _playerInput.Player.Move.canceled += OnMove;
        _playerInput.Player.Click.performed += OnClick;
        _playerInput.Player.CursorDrag.performed += OnCursorDrag;
    }

    private void OnDisable()
    {
        _playerInput.Player.Move.performed -= OnMove;
        _playerInput.Player.Move.canceled -= OnMove;
        _playerInput.Player.Click.performed -= OnClick;
        _playerInput.Player.CursorDrag.performed -= OnCursorDrag;
        
        _playerInput.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Vector3 direction = context.ReadValue<Vector3>();
        
        Moved?.Invoke(direction);
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = _playerInput.Player.CursorDrag.ReadValue<Vector2>();

        Clicked?.Invoke(mousePosition);
    }

    private void OnCursorDrag(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = context.ReadValue<Vector2>();

        if (mousePosition != _lastMousePosition)
        {
            _lastMousePosition = mousePosition;
            
            MousePositionChanged?.Invoke(mousePosition);
        }
    }
}