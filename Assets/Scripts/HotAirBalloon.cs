using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotAirBalloon : MonoBehaviour
{

	private Rigidbody rb;
	private float BalloonTemperature;
	private float BalloonPressure;
	private float MediumTemperature;

	public bool isBurnerOn = false;
	public bool isReleaseOn = false;

	private float height;

	private Vector3 force;


	public float AmbientTemperature;
	public float AmbientPressure;


    // Start is called before the first frame update
    void Start()
    {
		rb = GetComponent<Rigidbody>();
		AmbientTemperature = AtmosphereManager.GetAmbientTemperature(transform.position.y);
		BalloonTemperature = AmbientTemperature;
		AmbientPressure = AtmosphereManager.GetAmbientPressure(transform.position.y, BalloonTemperature);
		MediumTemperature = AmbientTemperature;
		BalloonPressure = AmbientPressure;
		height = 1f;
    }

	// Update is called once per frame
	void Update()
	{
		PassiveLoss();
		height = transform.position.y;
	}

    void FixedUpdate()
    {
		force = GetYForce(height);
		Debug.Log(force);
		Debug.Log(BalloonTemperature);
		Debug.Log(AmbientTemperature);
		rb.AddForce(force);
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
		/*float increment = GetTemperatureRate(773.15f, BalloonTemperature, GetBalloonDensity(), Time.fixedDeltaTime, 1f, 60f, AtmosphereManager.ThermalConductivity, AtmosphereManager.HeatCapacityAir);*/
		float increment = GetConvectionRate(800f, BalloonTemperature, GetBalloonDensity(), Time.fixedDeltaTime, 1f, AtmosphereManager.ThermalConvectivity, AtmosphereManager.HeatCapacityAir);
		BalloonTemperature += increment;
	}

	void PassiveLoss()
	{
		/*Assume Balloon Sphere, ~Physics*/
		MediumTemperature += GetTemperatureRate(BalloonTemperature, MediumTemperature, 1440, Time.deltaTime, 17400f, 0.5f, 0.04f, 1420f);

		MediumTemperature += GetTemperatureRate(AmbientTemperature, MediumTemperature, 1440, Time.deltaTime, 17650f, 0.5f, 0.04f, 1420f);

		BalloonTemperature += GetTemperatureRate(MediumTemperature, BalloonTemperature, GetBalloonDensity(), Time.deltaTime, 17400f, 74.5f, 0.04f, AtmosphereManager.HeatCapacityAir);
	}

	void ReleaseOn()
	{
		BalloonTemperature += GetConvectionRate(AmbientTemperature, BalloonTemperature, GetBalloonDensity(), Time.fixedDeltaTime, 8f, AtmosphereManager.ThermalConvectivity, AtmosphereManager.HeatCapacityAir);
	}

	static float GetConvectionRate(float T1, float T2, float density, float dt, float Area, float ThermalConvectivity, float HeatCapacity)
	{
		float Rate = ThermalConvectivity * Area * (T1 - T2);
		float dT = Rate * dt / (density * HeatCapacity);
		return dT;
	}

	static float GetTemperatureRate(float T1, float T2, float density, float dt, float Area, float Thickness, float ThermalConductivity, float HeatCapacity)
	{
		float Rate = ThermalConductivity * Area * (T1 - T2) / Thickness;
		float dT = Rate * dt / (density * HeatCapacity);
		return dT;
	}

	Vector3 GetYForce(float height)
	{
		return new Vector3(0f, (AtmosphereManager.GetAmbientDensity(height) - GetBalloonDensity()) * 2800f * 9.80665f, 0f);
	}

	float GetBalloonDensity()
	{
		return BalloonPressure / (AtmosphereManager.SpecficGasConstant * BalloonTemperature);
	}
}
