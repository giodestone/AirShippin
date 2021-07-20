using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AirshipCockpit : MonoBehaviour
{
    Burner burner;
    Release release;
	Propeller propeller;
    Rudder rudder;
    HotAirBalloon hotAirBalloon;

    JobManager jobManager;

    AirshipFuelCanisterItemHolder FuelInUse;

    [SerializeField] Transform envelope;

    AverageValues verticalSpeedAverage;

    [SerializeField] TextMeshProUGUI tempText;
    [SerializeField] TextMeshProUGUI altText;
    [SerializeField] TextMeshProUGUI verticalSpeedText;
    [SerializeField] TextMeshProUGUI curHDGSpeedText;
    [SerializeField] TextMeshProUGUI destHDGSpeedText;
    [SerializeField] TextMeshProUGUI fuelText;

    [SerializeField] Animator throttleAnimator;
    // [SerializeField] Animator steeringAnimator;


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
        hotAirBalloon = FindObjectOfType<HotAirBalloon>();
        jobManager = FindObjectOfType<JobManager>();
        verticalSpeedAverage = gameObject.AddComponent<AverageValues>();
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
        throttleAnimator.Play("Throttle", 0, throttle);

        // // -15 x max    -160x min
        // var minRotation = Quaternion.Euler(-15f, 0f, 0f);
        // var maxRotation = Quaternion.Euler(-160f, 0f, 0f);

        // throttleTransform.localRotation = Quaternion.Lerp(minRotation, maxRotation, throttle);
    }

    void UpdateSteeringModel()
    {
        // steeringAnimator.Play("Steering", 0, steering);
        // //-180x left, 0 right
        // var minRotation = Quaternion.Euler(0, 0f, 0f);
        // var maxRotation = Quaternion.Euler(-160f, 0f, 0f);

        // steeringWheelTransform.localRotation = Quaternion.LerpUnclamped(minRotation, maxRotation, steering + 0.5f);
    }

    void LateUpdate()
    {
        UpdateText();
    }

    float prevAltitude;

    void UpdateText()
    {
        verticalSpeedAverage.NewValue = (envelope.transform.position.y - prevAltitude) / Time.deltaTime;

        tempText.text = "BALLOON TEMP C: " + (hotAirBalloon.BalloonTemp - 273.15f).ToString("0.0");
        altText.text = "ALT: " + envelope.transform.position.y.ToString("0.00");
        verticalSpeedText.text = "VS/s: " + verticalSpeedAverage.MovingAverage.ToString("0.0");
        var dir = envelope.transform.forward - Vector3.forward;
        curHDGSpeedText.text = "HDG: " + (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg).ToString("0");

        var dirToTgt = "HDG->TGT: ";
        if (jobManager.Destination != null)
        {
            var envToDest = jobManager.Destination.transform.position - envelope.transform.position;
            var dirtotgtangle = envelope.transform.forward - envToDest.normalized;
            dirToTgt += (Mathf.Atan2(dirtotgtangle.y, dirtotgtangle.x) * Mathf.Rad2Deg).ToString("0");
        }
        else
        {
            dirToTgt += " NO TGT";
        }

        destHDGSpeedText.text = dirToTgt.ToString();

        fuelText.text = "FUEL: " + (FuelInUse.Fuel * 100f).ToString("0.0");

        prevAltitude = envelope.transform.position.y;
    }
}
