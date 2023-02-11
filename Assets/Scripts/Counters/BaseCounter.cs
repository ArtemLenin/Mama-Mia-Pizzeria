using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private Transform _counterTopPoint;

    public KitchenObject KitchenObject { get; set; }

    public virtual void Interact(Player player)
    {
        Debug.LogError("BaseCounter.Interact()");
    }

    public virtual void InteractAlternative(Player player)
    {
        //Debug.LogError("BaseCounter.InteractAlternative()");
    }

    public void ClearKitchenObject()
    {
        KitchenObject = null;
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return _counterTopPoint;
    }
}