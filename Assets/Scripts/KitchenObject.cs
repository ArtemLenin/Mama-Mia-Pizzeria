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

    public void DestroySelf()
    {
        KitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }

    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.Prefab);
        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        kitchenObject.KitchenObjectParent = kitchenObjectParent;

        return kitchenObject;
    }

    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if (this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        else
        {
            plateKitchenObject = null;
            return false;
        }
    }
}