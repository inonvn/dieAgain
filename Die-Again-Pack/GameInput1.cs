using UnityEngine;
using System;

public class GameInput1 : MonoBehaviour
{
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnToggleRecipeBookAction;

    private PlayerInputAction playerInputActions;
    public Joystick joystick;

    private void Awake()
    {
        playerInputActions = new PlayerInputAction();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;

        playerInputActions.Player.ToggleRecipeBook.performed += ToggleRecipeBook_performed;

    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    private void ToggleRecipeBook_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnToggleRecipeBookAction?.Invoke(this, EventArgs.Empty);
    }
    public Vector2 getMovementVectorNormalized()
    {

        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        if (joystick.input != Vector2.zero)
        {
            inputVector = joystick.input;
        }
        


        inputVector = inputVector.normalized;
        return inputVector;
    }

    private void OnDestroy()
    {

        playerInputActions.Player.Interact.performed -= Interact_performed;
        playerInputActions.Player.InteractAlternate.performed -= InteractAlternate_performed;
        playerInputActions.Player.ToggleRecipeBook.performed -= ToggleRecipeBook_performed;
        playerInputActions.Dispose();
    }
}
