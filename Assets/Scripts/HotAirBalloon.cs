using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotAirBalloon : MonoBehaviour
{

	private Rigidbody rb;
	private float BalloonTemperature;
	private float BalloonPressure;
	private float MediumTemperature;
	private float BalloonVolume;

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
		BalloonVolume = 26673f;
    }

	// Update is called once per frame
	void Update()
	{
		AmbientTemperature = AtmosphereManager.GetAmbientTemperature(height);
		Debug.Log(AmbientTemperature);
		AmbientPressure = AtmosphereManager.GetAmbientPressure(height, AmbientTemperature);
		PassiveLoss();
		height = transform.position.y;
	}

    void FixedUpdate()
    {
		force = GetYForce(height);
		Quaternion deltaQuat = Quaternion.FromToRotation(Quaternion.Euler(90f,0f,0f) * rb.transform.up, Vector3.up);

		Vector3 axis;
		float angle;

		deltaQuat.ToAngleAxis(out angle, out axis);

		float dampenFactor = 0.01f; // this value requires tuning
		rb.AddTorque(-rb.angularVelocity * dampenFactor, ForceMode.Acceleration);

		Debug.DrawLine(rb.transform.position, rb.transform.position + (rb.transform.right * 100f));

		float adjustFactor = 0.005f; // this value requires tuning
		rb.AddTorque(axis.normalized * angle * adjustFactor, ForceMode.Acceleration);
		rb.AddForce(force);

		if (isBurnerOn)
		{
			BurnerOn();
			AtmosphereManager.pollution += 1f;
		}
		if (isReleaseOn)
		{
			ReleaseOn();
		}
    }

	private float AngularCorrection(float rotationalVel)
	{
		return -1 * rotationalVel * rb.inertiaTensor.magnitude;
	}

	void BurnerOn()
	{
		/*float increment = GetTemperatureRate(773.15f, BalloonTemperature, GetBalloonDensity(), Time.fixedDeltaTime, 1f, 60f, AtmosphereManager.ThermalConductivity, AtmosphereManager.HeatCapacityAir);*/
		float increment = GetConvectionRate(800f, BalloonTemperature, GetBalloonDensity(), Time.fixedDeltaTime, 1f, AtmosphereManager.ThermalConvectivity, AtmosphereManager.HeatCapacityAir);
		BalloonTemperature += increment+0.5f;
	}

	void PassiveLoss()
	{
		/*Assume Balloon Sphere, ~Physics*/
		MediumTemperature += GetTemperatureRate(BalloonTemperature, MediumTemperature, 1440, Time.deltaTime, 4722f, 0.05f, 0.04f, 1420f);

		MediumTemperature += GetTemperatureRate(AmbientTemperature, MediumTemperature, 1440, Time.deltaTime, 4824f, 0.05f, 0.04f, 1420f);

		BalloonTemperature += GetTemperatureRate(MediumTemperature, BalloonTemperature, GetBalloonDensity(), Time.deltaTime, 4722f, 60f, 0.04f, AtmosphereManager.HeatCapacityAir);
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
		float AmbientDensity = AtmosphereManager.GetAmbientDensity(height);
		float BalloonDensity = GetBalloonDensity();
		return new Vector3(0f, (AmbientDensity - BalloonDensity) * BalloonVolume * 9.80665f, 0f);
	}

	float GetBalloonDensity()
	{
		return BalloonPressure / (AtmosphereManager.SpecficGasConstant * BalloonTemperature);
	}
}
