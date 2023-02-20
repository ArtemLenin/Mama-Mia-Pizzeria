using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private const string PLAYER_PREFS_BINDINGS = "InputBindings";
    public static GameInput Instance { get; private set; }

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternativeAction;
    public event EventHandler OnPauseAction;

    public enum Binding
    {
        MoveUp,
        MoveDown,
        MoveLeft,
        MoveRight,
        Interact,
        InteractAlternate,
        Pause,
        GamepadInteract,
        GamepadInteractAlternate,
        GamepadPause
    }
    private PlayerInputActions _actions;

    private void Awake()
    {
        Instance = this;


        _actions = new PlayerInputActions();
        
        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
        {
            _actions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }

        _actions.Player.Enable();

        _actions.Player.Interaction.performed += Interaction_performed;
        _actions.Player.InteractionAlternative.performed += InteractionAlternative_performed; ;
        _actions.Player.Pause.performed += Pause_performed;


    }

    private void OnDestroy()
    {
        _actions.Player.Interaction.performed -= Interaction_performed;
        _actions.Player.InteractionAlternative.performed -= InteractionAlternative_performed; ;
        _actions.Player.Pause.performed -= Pause_performed;

        _actions.Dispose();
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractionAlternative_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAlternativeAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interaction_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementNormalizedVector()
    {
        Vector2 inputVector = _actions.Player.Move.ReadValue<Vector2>();
        inputVector.Normalize();

        return inputVector;
    }

    public string GetBindingText(Binding binding)
    {
        switch (binding)
        {
            default:
            case Binding.MoveUp:
                return _actions.Player.Move.bindings[1].ToDisplayString();
            case Binding.MoveDown:
                return _actions.Player.Move.bindings[2].ToDisplayString();
            case Binding.MoveLeft:
                return _actions.Player.Move.bindings[3].ToDisplayString();
            case Binding.MoveRight:
                return _actions.Player.Move.bindings[4].ToDisplayString();
            case Binding.Interact:
                return _actions.Player.Interaction.bindings[0].ToDisplayString();
            case Binding.InteractAlternate:
                return _actions.Player.InteractionAlternative.bindings[0].ToDisplayString();
            case Binding.Pause:
                return _actions.Player.Pause.bindings[0].ToDisplayString();
            case Binding.GamepadInteract:
                return _actions.Player.Interaction.bindings[1].ToDisplayString();
            case Binding.GamepadInteractAlternate:
                return _actions.Player.InteractionAlternative.bindings[1].ToDisplayString();
            case Binding.GamepadPause:
                return _actions.Player.Pause.bindings[1].ToDisplayString();
        }
    }
    public void RebindBinding(Binding binding, Action onActionRebound)
    {
        _actions.Player.Disable();

        InputAction action;
        int bindingIndex;

        switch(binding)
        {
            default:
            case Binding.MoveUp:
                action = _actions.Player.Move;
                bindingIndex = 1;
                break;
            case Binding.MoveDown:
                action = _actions.Player.Move;
                bindingIndex = 2;
                break;
            case Binding.MoveLeft:
                action = _actions.Player.Move;
                bindingIndex = 3;
                break;
            case Binding.MoveRight:
                action = _actions.Player.Move;
                bindingIndex = 4;
                break;
            case Binding.Interact:
                action = _actions.Player.Interaction;
                bindingIndex = 0;
                break;
            case Binding.InteractAlternate:
                action = _actions.Player.InteractionAlternative;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                action = _actions.Player.Pause;
                bindingIndex = 0;
                break;
            case Binding.GamepadInteract:
                action = _actions.Player.Interaction;
                bindingIndex = 1;
                break;
            case Binding.GamepadInteractAlternate:
                action = _actions.Player.InteractionAlternative;
                bindingIndex = 1;
                break;
            case Binding.GamepadPause:
                action = _actions.Player.Pause;
                bindingIndex = 1;
                break;
        }

        action.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(callback =>
            {
                callback.Dispose();
                _actions.Player.Enable();
                onActionRebound();


                PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, _actions.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();
            }).Start();
    }
}