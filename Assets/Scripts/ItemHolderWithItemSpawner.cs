using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Item holder that spawns things when it is safe to do so.
/// </summary>
public class ItemHolderWithItemSpawner : ItemHolder
{
    [SerializeField] GameObject itemPrefab;
    [SerializeField] ObjectsInTriggerColliderCounter counter; // For checking if its possible to spawn stuff. Should be in an are where the thing will be spawned.
    
    [Header("-1 means infinite")]
    [SerializeField] int numToSpawn = -1; // yes -1 is bad but i cbb adding anything else
    public int NumToSpawn { get => numToSpawn; set => numToSpawn = value; }

    [SerializeField] bool shouldSpawn = true;
    public bool ShouldSpawn { get => shouldSpawn; set => shouldSpawn = value; }
    

    const float spawnDelayTime = 5f; // seconds

    void Start()
    {
        SpawnCanisterAfterDelay();
    }

    void SpawnCanisterAfterDelay()
    {
        Invoke("SpawnItemIfPossible", spawnDelayTime);
    }

    void SpawnItemIfPossible()
    {
        if (counter.IsAnythingInCollider || !ShouldSpawn) // Do not spawn if something in collider or shouldn't be spawning.
        {
            SpawnCanisterAfterDelay();
            return;
        }

        if (numToSpawn != -1 && numToSpawn <= 0) // Do not spawn if no items to spawn.
        {
            ShouldSpawn = false;
            SpawnCanisterAfterDelay();
            return;
        }

        SpawnItem();

        if (numToSpawn != -1)
            numToSpawn--;

        SpawnCanisterAfterDelay();
    }

    void SpawnItem()
    {
        var intanciatedObject = Instantiate(itemPrefab, parent: itemAttachmentPoint);
        PutItemIn(intanciatedObject);
        OnItemSpawned(intanciatedObject);
    }

    protected virtual void OnItemSpawned(GameObject obj)
    {

    }

    public override void TakeItemOut(GameObject item)
    {
        base.TakeItemOut(item);

        counter.RemoveObject(item.gameObject); // Cos the OnTriggerExit does not register otherwise.
    }

    public override void ItemHeldDestroyed()
    {
        counter.RemoveObject(CurrentItem.gameObject);
        base.ItemHeldDestroyed();
    }
}
