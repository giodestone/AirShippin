using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirshipCockpit : MonoBehaviour
{
    public void NotifyButtonPressStart(AirshipButtonAction airshipButtonAction)
    {
        // TODO
        // Debug.Log("Button Press: " + airshipButtonAction);

        switch (airshipButtonAction)
        {
            case AirshipButtonAction.BurnerStart:
                break;
            case AirshipButtonAction.VentStart:
                break;
        }
    }

    public void NotifyButtonPressEnd(AirshipButtonAction airshipButtonAction)
    {
        // TODO
        // Debug.Log("Button Press: " + airshipButtonAction);
        switch (airshipButtonAction)
        {
            case AirshipButtonAction.BurnerEnd:
                break;
            case AirshipButtonAction.VentEnd:
                break;
        }
    }

    public void UpdateSteering(float newValue)
    {
        // TODO
        // Debug.Log("Steering: " + newValue);
    }

    public void UpdateThrottle(float newValue)
    {
        // TODO
        // Debug.Log("Throttle: " + newValue);
    }
}
