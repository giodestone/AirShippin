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
        if (counter.IsAnythingInCollider)
        {
            Debug.Log("Cannot spawn something in way.");
            SpawnCanisterAfterDelay();
            return;
        }
        Debug.Log("Spawning!");

        var intanciatedObject = Instantiate(itemPrefab, parent: itemAttachmentPoint);
        PutItemIn(intanciatedObject);

        SpawnCanisterAfterDelay();
    }

    public override void TakeItemOut(GameObject item)
    {
        base.TakeItemOut(item);
        
        counter.RemoveObject(item.gameObject);
    }
}
