using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Base class for an ItemHolder.
/// </summary>
public class ItemHolder : MonoBehaviour
{
    [SerializeField] protected Transform itemAttachmentPoint;

    public bool IsHolderFull { get => currentItem != null; }
    public GameObject CurrentItem { get => currentItem; }
    GameObject currentItem;
    KeepInPlace keepInPlace;

    void Start()
    {
        Assert.IsTrue((gameObject.layer & LayerMask.NameToLayer("ItemPutDownSurface")) != 0, "Error: Layer mask is not set, the object will not be put down until layer mask is set to 'ItemPutDownSurface'.");
    }

    public virtual void PutItemIn(GameObject item)
    {
        currentItem = item;

        var itemInteractable = item.GetComponent<ItemInteractable>();
        if (itemInteractable != null)
            itemInteractable.ItemHolder = this;

        var rigidBody = item.GetComponent<Rigidbody>();
        if (rigidBody != null)
            rigidBody.isKinematic = true;

        item.transform.parent = itemAttachmentPoint;
        item.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        item.transform.position = Vector3.zero;

        keepInPlace = item.AddComponent<KeepInPlace>();
        keepInPlace.obj = item.transform;
        keepInPlace.target = itemAttachmentPoint;
        
        // SetItemCollisionState(item, false);
    }

    public void SetItemCollisionState(GameObject item, bool state)
    {
        var parent = item.transform.parent;
        var rigidbodies = parent.GetComponentsInChildren<Rigidbody>();

        foreach (var rb in rigidbodies)
            rb.detectCollisions = state;
    }

    public virtual void TakeItemOut(GameObject item)
    {
        if (item != currentItem)
        {
            Debug.LogWarning("Unable to remove an item that is not in the holder!");
            return;
        }

        var itemInteractable = item.GetComponent<ItemInteractable>();
        if (itemInteractable != null)
            itemInteractable.ItemHolder = null;

        var rigidBody = item.GetComponent<Rigidbody>();
        if (rigidBody != null)
            rigidBody.isKinematic = false;

        // SetItemCollisionState(currentItem, true);

        Destroy(keepInPlace);
        keepInPlace = null;

        item.transform.parent = null;
        currentItem = null;

    }

    /// <summary>
    /// Tell the item holder that the thing its holding has been destroyed.
    /// </summary>
    public virtual void ItemHeldDestroyed()
    {
        currentItem = null;
    }
}
