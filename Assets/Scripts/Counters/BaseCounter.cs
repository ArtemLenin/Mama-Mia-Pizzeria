using System;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private Transform _counterTopPoint;
    public static event EventHandler OnAnyObjectPlacedHere;
    private KitchenObject _kitchenObject;

    public KitchenObject KitchenObject
    {
        get
        {
            return _kitchenObject;
        }
        set
        {
            _kitchenObject = value;
            if (_kitchenObject) OnAnyObjectPlacedHere?.Invoke(this, EventArgs.Empty);
        }
    }

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