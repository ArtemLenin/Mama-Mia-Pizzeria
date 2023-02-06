using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }
    public KitchenObject KitchenObject { get; set; }

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public ClearCounter selectedCounter;
    }

    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private GameInput _gameInput;
    [SerializeField] private LayerMask _countersLayerMask;
    [SerializeField] private Transform _kitchenObjectHoldPoint;

    private bool _isWalking;
    private Vector3 _lastInteraction;
    private ClearCounter _selectedCounter;

    private void Awake()
    {
        if (Instance != null) Debug.LogError("There is more than one Player instance");
        Instance = this;
    }

    private void Start()
    {
        _gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (_selectedCounter) _selectedCounter.Interact(this);
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = _gameInput.GetMovementNormalizedVector();

        Vector3 direction = new Vector3(inputVector.x, 0f, inputVector.y);
        if (direction != Vector3.zero) _lastInteraction = direction;

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, _lastInteraction, out RaycastHit hit, interactDistance, _countersLayerMask))
        {
            if (hit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                if (clearCounter != _selectedCounter)
                {
                    SetSelectedCounter(clearCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }

    private void HandleMovement()
    {
        Vector2 inputVector = _gameInput.GetMovementNormalizedVector();

        Vector3 direction = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = _movementSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, direction, moveDistance);

        if (!canMove)
        {
            Vector3 directionX = (Vector3.right * direction.x).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, directionX, moveDistance);

            if (canMove) direction = directionX;
            else
            {
                Vector3 directionZ = (Vector3.forward * direction.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, directionZ, moveDistance);

                if (canMove)
                {
                    direction = directionZ;
                }
            }
        }

        if (canMove) transform.position += direction * _movementSpeed * Time.deltaTime;


        _isWalking = direction != Vector3.zero;

        transform.forward = Vector3.Slerp(transform.forward, direction, _rotationSpeed * Time.deltaTime);
    }

    public bool IsWalking()
    {
        return _isWalking;
    }

    private void SetSelectedCounter(ClearCounter selectedCounter)
    {
        _selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = _selectedCounter

        });
    }

    public void ClearKitchenObject()
    {
        KitchenObject = null;
}

    public Transform GetKitchenObjectFollowTransform()
    {
        return _kitchenObjectHoldPoint;
    }
}
