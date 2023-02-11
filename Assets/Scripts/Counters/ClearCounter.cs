using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO _kitchenObjectSO;

    public override void Interact(Player player)
    {
        if (!KitchenObject)
        {
            if (!player.KitchenObject) return;
            player.KitchenObject.KitchenObjectParent = this;
        }
        else
        {
            if (player.KitchenObject)
            {
                if (player.KitchenObject.TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if (plateKitchenObject.TryAddIngredient(KitchenObject.GetKitchenObjectSO()))
                    {
                        KitchenObject.DestroySelf();
                    }
                }
                else
                {
                    if (player.KitchenObject.TryGetPlate(out plateKitchenObject))
                    {
                        if (plateKitchenObject.TryAddIngredient(KitchenObject.GetKitchenObjectSO()))
                        {
                            player.KitchenObject.DestroySelf();
                        }
                    }
                }
            }
            else
            {
                KitchenObject.KitchenObjectParent = player;
            }
        }
    }
}