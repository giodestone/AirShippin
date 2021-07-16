using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burner : MonoBehaviour
{
    public void AttemptBurnStart()
    {
        // Delete me
        var envelope = GameObject.FindGameObjectWithTag("Envelope").GetComponent<Rigidbody>();
        envelope.AddForce(Vector3.zero, ForceMode.Force);
    }

    public void BurnStop()
    {

    }
}
