using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotAirBalloon : MonoBehaviour
{

	private Rigidbody rb;
	private float BalloonTemperature;
	private float BalloonPressure;
	private float MediumTemperature;
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
		MediumTemperature = AmbientTemperature;
		BalloonPressure = AmbientPressure;
    }

	// Update is called once per frame
	void Update()
	{
		PassiveLoss();
	}

    void FixedUpdate()
    {
		rb.AddForce(GetYForce());
		if (isBurnerOn)
		{
			BurnerOn();
		}
		if (isReleaseOn)
		{
			ReleaseOn();
		}
    }

	void BurnerOn()
	{
		BalloonTemperature += GetTemperatureRate(500f, BalloonTemperature, GetBalloonDensity(), Time.FixedDeltaTime, 1f, 60f, AtmosphereManager.ThermalConductivity, AtmosphereManager.HeatCapacityAir);
	}

	void PassiveLoss()
	{
		/*Assume Balloon Sphere, ~Physics*/
		MediumTemperature += GetTemperatureRate(BalloonTemperature, MediumTemperature, 1440, Time.DeltaTime, 17400f, 0.5f, 0.04f, 1420f);

		MediumTemperature += GetTemperatureRate(AmbientTemperature, MediumTemperature, 1440, Time.DeltaTime, 17650f, 0.5f, 0.04f, 1420f);

		BalloonTemperature += GetTemperatureRate(MediumTemperature, BalloonTemperature, GetBalloonDensity(), Time.DeltaTime, 17400f, 74.5f, 0.04);
	}

	void ReleaseOn()
	{
		BalloonTemperature += GetTemperatureRate(AmbientTemperature, BalloonTemperature, GetBalloonDensity(), Time.FixedDeltaTime, 8f, 60f, AtmosphereManager.ThermalConductivity, AtmosphereManager.HeatCapacityAir);
	}

	static float GetTemperatureRate(float T1, float T2, float density, float dt, float Area, float Thickness, float ThermalConductivity, float HeatCapacity)
	{
		float Rate = ThermalConductivity * Area * (T1 - T2) / Thickness;
		float dT = Rate * dt / (density * HeatCapacity);
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
