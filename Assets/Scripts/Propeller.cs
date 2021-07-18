using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Propeller : MonoBehaviour
{
    public Rigidbody rb;

    private float Power = 93212.48f;
    private float AppliedThrust;
    public float ThrottleValue { get; set; }

    [SerializeField] GameObject Envelope;

    private Vector3 TotalThrust;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ThrottleValue = 0f;
        Envelope = GameObject.Find("Envelope");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float velocity = Vector3.Dot(rb.velocity, -1*Envelope.transform.up.normalized);
        float AppliedPower = Power * ThrottleValue;
        if (velocity > 0f)
        {
            AppliedThrust = AppliedPower / (velocity);
            AtmosphereManager.pollution += 1f * ThrottleValue;
        }
        TotalThrust = -1*Envelope.transform.up * AppliedThrust;
        rb.AddForce(TotalThrust);
    }
}
