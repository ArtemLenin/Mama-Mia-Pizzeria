using UnityEngine;

public class TrashCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        if (player.KitchenObject) player.KitchenObject.DestroySelf();
    }
}