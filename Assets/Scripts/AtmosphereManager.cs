using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AtmosphereManager
{

    const float RefTemperature = 293f;
    const float RefHeight = 0f;
    const float RefPressure = 101325f;

    public static float GetAmbientTemperature(float height)
    {
		return RefTemperature - 0.0065f * (height - RefHeight);
    }

    public static float GetAmbientPressure(float height, float AmbientTemperature)
    {
		return Mathf.Pow(RefPressure * (1f - (0.0065f * height) / (AmbientTemperature + 0.0065f * height)), 5.257f);
    }

	public static float GetAmbientDensity()
	{
		throw new NotImplementedException();
	}

}
