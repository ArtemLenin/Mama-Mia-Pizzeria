using System;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;
    public static DeliveryManager Instance { get; private set; }
    [SerializeField] private RecipeListSO _recipeListSO;
    private List<RecipeSO> _waitingRecipeSOList;

    private float _spawnRecipeTimer;
    private float _spawnRecipeTimerMax = 4f;
    private int _waitingRecipeMax = 4;
    private int _successfulRecipesAmount;

    public int GetSuccessfulRecipesAmount() => _successfulRecipesAmount;

    private void Awake()
    {
        Instance = this;
        _waitingRecipeSOList = new List<RecipeSO>();
    }

    private void Update()
    {
        _spawnRecipeTimer -= Time.deltaTime;
        if (_spawnRecipeTimer <= 0f)
        {
            _spawnRecipeTimer = _spawnRecipeTimerMax;

            if (KitchenGameManager.Instance.IsGamePlaying() && _waitingRecipeSOList.Count < _waitingRecipeMax)
            {
                RecipeSO waitingRecipeSO = _recipeListSO.RecipeSOList[UnityEngine.Random.Range(0, _recipeListSO.RecipeSOList.Count)];
                _waitingRecipeSOList.Add(waitingRecipeSO);

                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }

    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        foreach (RecipeSO recipe in _waitingRecipeSOList)
        {
            if (recipe.KitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                bool plateContentsMathesRecipe = true;
                foreach (KitchenObjectSO itemRecipe in recipe.KitchenObjectSOList)
                {
                    bool ingredientFound = false;
                    foreach (KitchenObjectSO itemPlate in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        if (itemPlate == itemRecipe)
                        {
                            ingredientFound = true;
                            break;
                        }
                    }
                    if (!ingredientFound)
                    {
                        plateContentsMathesRecipe = false;
                    }
                }
                if (plateContentsMathesRecipe)
                {
                    _successfulRecipesAmount++;
                    _waitingRecipeSOList.Remove(recipe);
                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
        }
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }

    public List<RecipeSO> GetWaitingSOList() => _waitingRecipeSOList;
}