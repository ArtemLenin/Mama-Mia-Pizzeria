using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO _kitchenObjectSO;
    private IKitchenObjectParent _kitchenObjectParent;
    public IKitchenObjectParent KitchenObjectParent
    {
        get 
        {
            return _kitchenObjectParent;
        }
        set
        {
            if (_kitchenObjectParent != null) _kitchenObjectParent.ClearKitchenObject();
            _kitchenObjectParent = value;
            if (_kitchenObjectParent.KitchenObject) Debug.LogError("IKitchenObjectParent already has a Kitchen Object");
            KitchenObjectParent.KitchenObject = this;

            transform.parent = KitchenObjectParent.GetKitchenObjectFollowTransform();
            transform.localPosition = Vector3.zero;
        }
    }

    public KitchenObjectSO GetKitchenObjectSO() => _kitchenObjectSO;
}