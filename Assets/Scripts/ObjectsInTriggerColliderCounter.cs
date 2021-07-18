using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps track if things are in collider.
/// </summary>
[RequireComponent(typeof(Collider))]
public class ObjectsInTriggerColliderCounter : MonoBehaviour
{
    HashSet<GameObject> objectsInCollider;

    public bool IsAnythingInCollider { get => objectsInCollider.Count > 0; }

    void Start()
    {
        objectsInCollider = new HashSet<GameObject>();
    }

    void OnTriggerEnter(Collider col)
    {
        objectsInCollider.Add(col.gameObject);
    }

    void OnTriggerExit(Collider col)
    {
        RemoveObject(col.gameObject);
    }

    /// <summary>
    /// Remove an object form a collider.
    /// </summary>
    /// <param name="gameObject"></param>
    public void RemoveObject(GameObject gameObject)
    {
        objectsInCollider.Remove(gameObject);
    }
}
