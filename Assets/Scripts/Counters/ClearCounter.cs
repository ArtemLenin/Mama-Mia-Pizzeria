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
            if (player.KitchenObject) return;
            KitchenObject.KitchenObjectParent = player;
        }
    }
}