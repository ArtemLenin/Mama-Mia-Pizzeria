using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public static DeliveryCounter Instance { get; private set; }

    private void Awake()
    {
        Instance= this;
    }

    public override void Interact(Player player)
    {
        if (player.KitchenObject)
        {
            if (player.KitchenObject.TryGetPlate(out PlateKitchenObject plate)) 
            {
                DeliveryManager.Instance.DeliverRecipe(plate);
                player.KitchenObject.DestroySelf();
            }
        }
    }
}