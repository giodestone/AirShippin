using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Base class for an ItemHolder.
/// </summary>
public class ItemHolder : MonoBehaviour
{
    void Start()
    {
        Assert.IsTrue(gameObject.layer == LayerMask.GetMask("ItemPutDownSurface"), "Error: Layer mask is not set, the object will not be put down until layer mask is set to 'ItemPutDownSurface'.");
    }

    public virtual void PutItemIn(GameObject item)
    {
        throw new NotImplementedException();
    }

    public virtual void TakeItemOut(GameObject item)
    {
        throw new NotImplementedException();
    }
}
