using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
	const float SpecficGasConstant = 287f;

	private Rigidbody rb;
	private float BalloonTemperature;
	private float BalloonPressure;

    // Start is called before the first frame update
    void Start()
    {
		rb = GetComponent<Rigidbody>();

		BalloonTemperature = AtmosphereManager.GetAmbientTemperature(transform.position.y);
		BalloonPressure = AtmosphereManager.GetAmbientPressure(transform.position.y, 273.15f/*ToDo*/);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
		rb.AddForce(GetForce());
    }

	Vector3 GetForce()
	{
		return new Vector3(0f, (AtmosphereManager.GetAmbientDensity() - GetBalloonDensity()) * 2800f * 9.80665f, 0f);
	}

	float GetBalloonDensity()
	{
		return BalloonPressure / (SpecficGasConstant * BalloonTemperature);
	}
}
