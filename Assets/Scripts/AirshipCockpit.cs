using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirshipCockpit : MonoBehaviour
{
    Burner burner;
    Release release;
    Propeller propeller;

    void Start()
    {
        burner = GetComponent<Burner>();
        release = GetComponent<Release>();
        propeller = GetComponent<Propeller>();
    }

    public void NotifyButtonPressStart(AirshipButtonAction airshipButtonAction)
    {
        // TODO
        // Debug.Log("Button Press: " + airshipButtonAction);

        switch (airshipButtonAction)
        {
            case AirshipButtonAction.BurnerStart:
                burner.AttemptBurnStart();
                break;
            case AirshipButtonAction.VentStart:
                release.ValveStart();
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
                burner.BurnStop();
                break;
            case AirshipButtonAction.VentEnd:
                release.ValveStop();
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
        propeller.CalcThrust(newValue);
    }
}
