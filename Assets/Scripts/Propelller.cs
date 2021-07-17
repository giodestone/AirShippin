using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Propeller : MonoBehaviour
{
    public Rigidbody rb;

    private float MaxThrust = 10000f;
    private float AppliedThrust;    
    private Vector3 TotalThrust;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    public void CalcThrust(float ThrottleValue)
    {
        if (ThrottleValue > 0f)
        {
            AppliedThrust = (MaxThrust + AppliedThrust) / 2;
            TotalThrust = new Vector3(AppliedThrust, 0f, 0f);
            rb.AddRelativeForce(TotalThrust);
        }
    }
}
