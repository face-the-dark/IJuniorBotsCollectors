using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    private PlayerInput _playerInput;

    public event Action<Vector3> Moved;
    public event Action<Vector2> Clicked;
    
    private void Awake() => 
        _playerInput = new PlayerInput();

    private void OnEnable()
    {
        _playerInput.Enable();
        
        _playerInput.Player.Move.performed += OnMove;
        _playerInput.Player.Move.canceled += OnMove;
        _playerInput.Player.Click.performed += OnClick;
    }

    private void OnDisable()
    {
        _playerInput.Player.Move.performed -= OnMove;
        _playerInput.Player.Move.canceled -= OnMove;
        _playerInput.Player.Click.performed -= OnClick;
        
        _playerInput.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Vector3 direction = context.ReadValue<Vector3>();
        
        Moved?.Invoke(direction);
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = _playerInput.Player.MousePosition.ReadValue<Vector2>();

        Clicked?.Invoke(mousePosition);
    }
}