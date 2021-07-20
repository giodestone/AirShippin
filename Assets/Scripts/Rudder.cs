using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rudder : MonoBehaviour
{

    public float steeringValue { get; set; }

    public Rigidbody rb;
    private Rigidbody envelope;

    // Start is called before the first frame update
    void Start()
    {
        steeringValue = 0f;
        rb = GetComponent<Rigidbody>();
        envelope = GameObject.Find("Envelope").GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float velocity = Vector3.Dot(rb.velocity, -1f * transform.up.normalized);
        float turningForce = velocity * steeringValue * 5000f;
        envelope.AddTorque(transform.forward * turningForce);
    }
}
