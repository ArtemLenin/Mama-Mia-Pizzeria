using System;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }
    public event EventHandler OnPickedSomething;
    private KitchenObject _kitchenObject;

    public KitchenObject KitchenObject
    {
        get
        {
            return _kitchenObject;
        }
        set
        {
            _kitchenObject = value;
            if (_kitchenObject) OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private LayerMask _countersLayerMask;
    [SerializeField] private Transform _kitchenObjectHoldPoint;

    private bool _isWalking;
    private Vector3 _lastInteraction;
    private BaseCounter _selectedCounter;

    private void Awake()
    {
        //Instance = this;
    }

    private void Start()
    {
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
        GameInput.Instance.OnInteractAlternativeAction += GameInput_OnInteractAlternativeAction; ;
    }

    private void GameInput_OnInteractAlternativeAction(object sender, EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;
        if (_selectedCounter) _selectedCounter.InteractAlternative(this);
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;
        if (_selectedCounter) _selectedCounter.Interact(this);
    }

    private void Update()
    {
        if (!IsOwner) return;
        HandleMovement();
        HandleInteractions();
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementNormalizedVector();

        Vector3 direction = new Vector3(inputVector.x, 0f, inputVector.y);
        if (direction != Vector3.zero) _lastInteraction = direction;

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, _lastInteraction, out RaycastHit hit, interactDistance, _countersLayerMask))
        {
            if (hit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if (baseCounter != _selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
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
        Vector2 inputVector = GameInput.Instance.GetMovementNormalizedVector();

        Vector3 direction = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = _movementSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, direction, moveDistance);

        if (!canMove)
        {
            Vector3 directionX = (Vector3.right * direction.x).normalized;
            canMove = (direction.x < -0.5f || direction.x > 0.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, directionX, moveDistance);

            if (canMove) direction = directionX;
            else
            {
                Vector3 directionZ = (Vector3.forward * direction.z).normalized;
                canMove = (direction.z < -0.5f || direction.z > 0.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, directionZ, moveDistance);

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

    private void SetSelectedCounter(BaseCounter selectedCounter)
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
