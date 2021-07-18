using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Propeller : MonoBehaviour
{
    public Rigidbody rb;

    private float Power = 93212.48f;
    private float AppliedThrust;
    public float ThrottleValue { get; set; }

    private Vector3 TotalThrust;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ThrottleValue = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float velocity = Vector3.Dot(rb.velocity, transform.forward.normalized);
        float AppliedPower = Power * ThrottleValue;
        if (velocity > 0f)
        {
            AppliedThrust = AppliedPower / (velocity);
        }
        TotalThrust = rb.transform.forward * AppliedThrust;
        rb.AddForce(TotalThrust);
    }
}
