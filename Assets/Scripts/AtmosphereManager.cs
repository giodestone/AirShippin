using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtmosphereManager : MonoBehaviour
{

    const float RefTemperature = 293f;
    const float RefHeight = 0f;
    const float RefPressure = 101325f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    static float AmbientTemperature(float height)
    {
        return RefTemperature - 0.0065(height - RefHeight)
    }

    static float AmbientPressure(float height, float AmbientTemperature)
    {
        return  Math.Pow(RefPressure(1-(0.0065*height)/(AmbientTemperature + 0.0065*height)), 5.257)
    }


}
