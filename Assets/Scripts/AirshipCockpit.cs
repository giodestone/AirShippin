using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirshipCockpit : MonoBehaviour
{
    Burner burner;
    Release release;
	Propeller propeller;
    Rudder rudder;

    AirshipFuelCanisterItemHolder FuelInUse;

    [SerializeField] Transform steeringWheelTransform;
    [SerializeField] Transform throttleTransform;

    private float throttle;
    private float steering;

	void Start()
    {
        burner = GetComponent<Burner>();
        release = GetComponent<Release>();
		propeller = GameObject.FindObjectOfType<Propeller>();
        rudder = GameObject.FindObjectOfType<Rudder>();
        FuelInUse = GameObject.FindObjectOfType<AirshipFuelCanisterItemHolder>();
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

        UpdateSteeringModel();

    }

    public void UpdateThrottle(float newValue)
    {
        // TODO
        // Debug.Log("Throttle: " + newValue);
        float fuel = FuelInUse.Fuel;
        if (fuel > 0f)
        {
            throttle += newValue * Mathf.Log10(fuel * 10f + 1f);
            throttle = Mathf.Clamp(throttle, -0.15f, 1f);
            propeller.ThrottleValue = throttle;
        }

        UpdateThrottleModel();
    }

    void UpdateThrottleModel()
    {
        // -15 x max    -160x min
        var minRotation = Quaternion.Euler(-15f, 0f, 0f);
        var maxRotation = Quaternion.Euler(-160f, 0f, 0f);

        throttleTransform.localRotation = Quaternion.Lerp(minRotation, maxRotation, throttle);
    }

    void UpdateSteeringModel()
    {
        //-180x left, 0 right
        var minRotation = Quaternion.Euler(0, 0f, 0f);
        var maxRotation = Quaternion.Euler(-160f, 0f, 0f);

        steeringWheelTransform.localRotation = Quaternion.LerpUnclamped(minRotation, maxRotation, steering + 0.5f);
    }
}
