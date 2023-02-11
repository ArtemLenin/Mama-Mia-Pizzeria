using System;
using UnityEngine;


public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }

    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned
    }

    [SerializeField] private FryingRecipeSO[] _fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] _burningRecipeSOArray;

    private State _currentState;
    private float _fryingTimer;
    private float _burningTimer;
    private FryingRecipeSO _fryingRecipeSO;
    private BurningRecipeSO _burningRecipeSO;

    private void Start()
    {
        _currentState = State.Idle;
    }

    private void Update()
    {
        if (!KitchenObject) return;

        switch (_currentState)
        {
            case State.Idle:
                break;
            case State.Frying:
                _fryingTimer += Time.deltaTime;

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    ProgressNormalized = _fryingTimer / _fryingRecipeSO.FryingTimerMax
                });

                if (_fryingTimer > _fryingRecipeSO.FryingTimerMax)
                {
                    KitchenObject.DestroySelf();
                    KitchenObject.SpawnKitchenObject(_fryingRecipeSO.Output, this);
                    _currentState = State.Fried;
                    _burningTimer = 0f;
                    _burningRecipeSO = GetBurningRecipeSOWithInput(KitchenObject.GetKitchenObjectSO());

                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs()
                    {
                        state = _currentState
                    });
                }
                break;
            case State.Fried:
                _burningTimer += Time.deltaTime;

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    ProgressNormalized = _burningTimer / _burningRecipeSO.BurningTimerMax
                });

                if (_burningTimer > _burningRecipeSO.BurningTimerMax)
                {
                    KitchenObject.DestroySelf();
                    KitchenObject.SpawnKitchenObject(_burningRecipeSO.Output, this);
                    _currentState = State.Burned;

                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs()
                    {
                        state = _currentState
                    });

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        ProgressNormalized = 0f
                    });
                }
                break;
            case State.Burned:
                break;
        }

    }

    public override void Interact(Player player)
    {
        if (!KitchenObject)
        {
            if (!player.KitchenObject) return;
            player.KitchenObject.KitchenObjectParent = this;
            _fryingRecipeSO = GetFryingRecipeSOWithInput(KitchenObject.GetKitchenObjectSO());
            _currentState = State.Frying;
            _fryingTimer = 0f;

            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs()
            {
                state = _currentState
            });
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                ProgressNormalized = _fryingTimer / _fryingRecipeSO.FryingTimerMax
            });
        }
        else
        {
            if (player.KitchenObject)
            {
                if (player.KitchenObject.TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    PlateKitchenObject plate = player.KitchenObject as PlateKitchenObject;
                    if (plate.TryAddIngredient(KitchenObject.GetKitchenObjectSO()))
                    {
                        KitchenObject.DestroySelf();

                        _currentState = State.Idle;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs()
                        {
                            state = _currentState
                        });

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            ProgressNormalized = 0f
                        });
                    }
                }
            }
            else
            {

                KitchenObject.KitchenObjectParent = player;
                _currentState = State.Idle;

                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs()
                {
                    state = _currentState
                });

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    ProgressNormalized = 0f
                });
            }
        }
    }

    //private bool HasRecipeWithInput(KitchenObjectSO input)
    //{
    //    return GetFryingRecipeSOWithInput(input) ? true : false
    //}

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        if (fryingRecipeSO) return fryingRecipeSO.Output;
        else return null;
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO recipe in _fryingRecipeSOArray)
        {
            if (recipe.Input == inputKitchenObjectSO) return recipe;
        }
        return null;
    }

    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO recipe in _burningRecipeSOArray)
        {
            if (recipe.Input == inputKitchenObjectSO) return recipe;
        }
        return null;
    }
}