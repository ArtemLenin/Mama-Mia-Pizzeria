using System;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    [SerializeField] private float _spawnPlateTimerMax;
    [SerializeField] private KitchenObjectSO _plateKitchenObjectSO;
    [SerializeField] int _platesSpawnedAmountMax;

    private float _spawnPlateTimer;
    private int _platesSpawnedAmount;
    

    private void Update()
    {
        _spawnPlateTimer += Time.deltaTime;
        if (_spawnPlateTimer > _spawnPlateTimerMax)
        {
            _spawnPlateTimer = 0;
            if (_platesSpawnedAmount < _platesSpawnedAmountMax)
            {
                _platesSpawnedAmount++;
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player)
    {
        if (player.KitchenObject) return;

        if (_platesSpawnedAmount > 0)
        {
            _platesSpawnedAmount--;
            KitchenObject.SpawnKitchenObject(_plateKitchenObjectSO, player);

            OnPlateRemoved?.Invoke(this, EventArgs.Empty);
        }
    }
}
