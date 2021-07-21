using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Propeller : MonoBehaviour
{
    public Rigidbody rb;

    private float Power = 150000f;
    private float AppliedThrust;
    AirshipFuelCanisterItemHolder FuelInUse;
    public float ThrottleValue { get; set; }

    [SerializeField] GameObject Envelope;

    private Vector3 TotalThrust;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ThrottleValue = 0f;
        Envelope = GameObject.Find("Envelope");
        FuelInUse = GameObject.FindObjectOfType<AirshipFuelCanisterItemHolder>();
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

        FuelInUse.Fuel -= Mathf.Abs(ThrottleValue * 0.01f * Time.fixedDeltaTime);

        TotalThrust = -1*Envelope.transform.up * AppliedThrust;
        Debug.DrawLine(transform.position, transform.position + (-1*  Envelope.transform.up * 100f));
        rb.AddForce(TotalThrust);
    }
}
