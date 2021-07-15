using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirshipCockpit : MonoBehaviour
{
    public void NotifyButtonPressStart(AirshipButtonAction airshipButtonAction)
    {
        // TODO
        Debug.Log("Button Press: " + airshipButtonAction);
    }

    public void NotifyButtonPressEnd(AirshipButtonAction airshipButtonAction)
    {
        // TODO
        Debug.Log("Button Press: " + airshipButtonAction);
    }

    public void UpdateSteering(float newValue)
    {
        // TODO
        Debug.Log("Steering: " + newValue);
    }

    public void UpdateThrottle(float newValue)
    {
        // TODO
        Debug.Log("Throttle: " + newValue);
    }
}
