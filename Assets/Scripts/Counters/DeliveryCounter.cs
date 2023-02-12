using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        if (player.KitchenObject)
        {
            if (player.KitchenObject.TryGetPlate(out PlateKitchenObject plate)) 
            {
                player.KitchenObject.DestroySelf();
            }
        }
    }
}