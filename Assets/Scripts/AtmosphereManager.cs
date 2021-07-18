using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AtmosphereManager
{

    const float RefTemperature = 293f;
    const float RefHeight = 0f;
    const float RefPressure = 101325f;

	public const float SpecficGasConstant = 287f;
	public const float HeatCapacityAir = 716f;
	public const float ThermalConductivity = 0.024f;
	public const float ThermalConvectivity = 2f;

	public static float pollution = 0.0f;

	//public static void climatechange(float fuelconsumption)
	//{
	//	/*todo*/
	//	pollution = fuelconsumption * 0.5f;
	//}

	public static float GetAmbientTemperature(float height)
    {
		return RefTemperature - 0.0065f * (height - RefHeight) + (pollution * 0.02f);
    }

    public static float GetAmbientPressure(float height, float AmbientTemperature)
    {
		float eqn = 1f - (0.0065f * height) / (AmbientTemperature + 0.0065f * height);
		return RefPressure * Mathf.Pow(eqn, 5.257f);
    }

	public static float GetAmbientDensity(float height)
	{
		float AmbientTemperature = GetAmbientTemperature(height);
		float AmbientPressure = GetAmbientPressure(height, AmbientTemperature);
		return AmbientPressure / (SpecficGasConstant * AmbientTemperature);
	}

}
