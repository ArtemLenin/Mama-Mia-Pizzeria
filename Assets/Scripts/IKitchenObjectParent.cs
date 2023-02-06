using UnityEngine;

public interface IKitchenObjectParent
{
    public KitchenObject KitchenObject { get; set; }

    public void ClearKitchenObject();
    public Transform GetKitchenObjectFollowTransform();

}
