using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirshipCockpit : MonoBehaviour
{
    Burner burner;
    Release release;
	Propeller propeller;
    Rudder rudder;

    private float throttle;
    private float steering;

	void Start()
    {
        burner = GetComponent<Burner>();
        release = GetComponent<Release>();
		propeller = GameObject.FindObjectOfType<Propeller>();
        rudder = GameObject.FindObjectOfType<Rudder>();
        throttle = 0f;
        steering = 0f;
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
        steering += newValue;
        steering = Mathf.Clamp(steering, -1f, 1f);
        rudder.steeringValue = steering;

    }

    public void UpdateThrottle(float newValue)
    {
        // TODO
        // Debug.Log("Throttle: " + newValue);
        throttle += newValue;
        throttle = Mathf.Clamp(throttle, -0.15f, 1f);
        propeller.ThrottleValue = throttle;
	}
}
