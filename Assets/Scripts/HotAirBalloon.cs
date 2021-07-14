using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotAirBalloon
{

	private Rigidbody rb;
	private float BalloonTemperature;
	private float BalloonPressure;

    // Start is called before the first frame update
    void Start()
    {
		rb = GetComponent<Rigidbody>();

		BalloonTemperature = AtmosphereManager.GetAmbientTemperature(transform.position.y);
		BalloonPressure = AtmosphereManager.GetAmbientPressure(transform.position.y, BalloonTemperature);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
		rb.AddForce(GetYForce());
    }

	Vector3 GetYForce()
	{
		return new Vector3(0f, (AtmosphereManager.GetAmbientDensity() - GetBalloonDensity()) * 2800f * 9.80665f, 0f);
	}

	float GetBalloonDensity()
	{
		return BalloonPressure / (AtmosphereManager.SpecficGasConstant * BalloonTemperature);
	}
}
