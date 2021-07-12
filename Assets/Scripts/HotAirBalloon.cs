using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
	const float SpecficGasConstant = 287;

	private Rigidbody rb;
	private float BalloonTemperature;
	private float BalloonPressure;

    // Start is called before the first frame update
    void Start()
    {
		rb = GetComponent<Rigidbody>();

		BalloonTemperature = AtmosphereManager.AmbientTemperature(transform.position.y);
		BalloonPressure = AtmosphereManager.AmbientPressure(transform.position.y);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
		rb.AddForce(GetForce);
    }

	static Vector3 GetForce()
	{
		return (0f, (AtmosphereManager.GetAmbientDensity - GetBalloonDensity) * 2800 * 9.80665, 0f);
	}

	static float GetBalloonDensity()
	{
		return BalloonPressure / (SpecficGasConstant * BalloonTemperature);
	}
}
