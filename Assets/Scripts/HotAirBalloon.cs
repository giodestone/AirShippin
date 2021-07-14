using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotAirBalloon
{

	private Rigidbody rb;
	private float BalloonTemperature;
	private float BalloonPressure;
	private bool isBurnerOn = false;
	private bool isReleaseOn = false;
	public float AmbientTemperature;
	public float AmbientPressure;

    // Start is called before the first frame update
    void Start()
    {
		rb = GetComponent<Rigidbody>();
		AmbientTemperature = AtmosphereManager.GetAmbientTemperature(transform.position.y);
		AmbientPressure = AtmosphereManager.GetAmbientPressure(transform.position.y, BalloonTemperature);
		BalloonTemperature = AmbientTemperature;
		BalloonPressure = AmbientPressure;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		rb.AddForce(GetYForce());
    }

	void BurnerOn()
	{

		BalloonTemperature += GetTemperatureRate();
	}

	void BurnerOff()
	{

	}

	void ReleaseOn()
	{

	}

	void ReleaseOff()
	{

	}

	static float GetTemperatureRate(float T1, float T2, float density, float dt, float Area, float Thickness)
	{
		float Rate = AtmosphereManager.ThermalConductivity * Area * (T1 - T2) / Thickness;
		float dT = Rate * dt / (density * AtmosphereManager.HeatCapacityAir);
		return dT;
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
