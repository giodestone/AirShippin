using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Move object to target every frame.
/// </summary>
public class KeepInPlace : MonoBehaviour
{
    [SerializeField] public Transform obj;
    [SerializeField] public Transform target;

    void Update()
    {
        obj.position = target.position;
        obj.rotation = target.rotation;
    }
}
