using System;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnPlayerGrabbedObject;

    [SerializeField] private KitchenObjectSO _kitchenObjectSO;

    public override void Interact(Player player)
    {
        if (player.KitchenObject) return;
        Transform kitchenObjectTransform = Instantiate(_kitchenObjectSO.Prefab);
        kitchenObjectTransform.GetComponent<KitchenObject>().KitchenObjectParent = player;
        OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
    }
}