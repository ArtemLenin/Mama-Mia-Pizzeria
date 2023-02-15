using System;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    public static event EventHandler OnAnyCut;

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnCut;

    [SerializeField] private CuttingRecipeSO[] _cuttingRecipeSOArray;
    private int _cuttingProgress;

    public override void Interact(Player player)
    {
        if (!KitchenObject)
        {
            if (!player.KitchenObject) return;
            player.KitchenObject.KitchenObjectParent = this;
            _cuttingProgress = 0;

            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(KitchenObject.GetKitchenObjectSO());

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs()
            {
                ProgressNormalized = (float)_cuttingProgress / cuttingRecipeSO.CuttingProgressMax
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
                    }
                }
            }
            else
            {
                KitchenObject.KitchenObjectParent = player;
            }
        }
    }

    public override void InteractAlternative(Player player)
    {
        if (KitchenObject && GetOutputForInput(KitchenObject.GetKitchenObjectSO()))
        {
            _cuttingProgress++;
            OnCut?.Invoke(this, EventArgs.Empty);
            OnAnyCut?.Invoke(this, EventArgs.Empty);

            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(KitchenObject.GetKitchenObjectSO());

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs()
            {
                ProgressNormalized = (float)_cuttingProgress / cuttingRecipeSO.CuttingProgressMax
            });

            if (_cuttingProgress >= cuttingRecipeSO.CuttingProgressMax)
            {
                KitchenObjectSO outputkitchenObjectSO = GetOutputForInput(KitchenObject.GetKitchenObjectSO());
                KitchenObject.DestroySelf();
                KitchenObject.SpawnKitchenObject(outputkitchenObjectSO, this);
            }
        }
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        if (cuttingRecipeSO) return cuttingRecipeSO.Output;
        else return null;
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO recipe in _cuttingRecipeSOArray)
        {
            if (recipe.Input == inputKitchenObjectSO) return recipe;
        }
        return null;
    }
}