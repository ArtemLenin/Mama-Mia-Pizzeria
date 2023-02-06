using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private KitchenObjectSO _kitchenObjectSO;
    [SerializeField] private Transform _counterTopPoint;

    public KitchenObject KitchenObject { get; set; }

    public void Interact(Player player)
    {
        if (!KitchenObject)
        {
            Transform kitchenObjectTransform = Instantiate(_kitchenObjectSO.Prefab, GetKitchenObjectFollowTransform());
            kitchenObjectTransform.GetComponent<KitchenObject>().KitchenObjectParent = this;
        }
        else
        {
            // Give the object to the player
            KitchenObject.KitchenObjectParent = player;
        }
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