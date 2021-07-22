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

    AverageValues verticalSpeedAverage;
    [SerializeField] Transform envelope;
    [SerializeField] TextMeshProUGUI tempText;
    [SerializeField] TextMeshProUGUI altText;
    [SerializeField] TextMeshProUGUI verticalSpeedText;
    [SerializeField] TextMeshProUGUI curHDGSpeedText;
    [SerializeField] TextMeshProUGUI destHDGSpeedText;
    [SerializeField] TextMeshProUGUI fuelText;

    [SerializeField] Animator throttleAnimator;
    [SerializeField] Animator steeringAnimator;


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

        UpdateSteeringModel();
        UpdateThrottleModel();
    }

    void Update()
    {
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

    public void UpdateSteering(float incrementValue)
    {
        steering += incrementValue;
        steering = Mathf.Clamp(steering, -1f, 1f);

        rudder.steeringValue = steering;
        
        UpdateSteeringModel();
    }

    public void UpdateThrottle(float incrementValue)
    {
        throttle += incrementValue;
        throttle = Mathf.Clamp(throttle, -0.2f, 1f);

        propeller.ThrottleValue = throttle;

        UpdateThrottleModel();
    }

    void UpdateThrottleModel()
    {
        throttleAnimator.Play("Throttle", 0, Mathf.Clamp(throttle, 0.0001f, 0.9999f));

        // // -15 x max    -160x min
        // var minRotation = Quaternion.Euler(-15f, 0f, 0f);
        // var maxRotation = Quaternion.Euler(-160f, 0f, 0f);

        // throttleTransform.localRotation = Quaternion.Lerp(minRotation, maxRotation, throttle);
    }

    void UpdateSteeringModel()
    {
        steeringAnimator.Play("Steering", 0, Mathf.Clamp((steering / 2f) + 0.5f, 0.0001f, 0.9999f));
        // //-180x left, 0 right
        // var minRotation = Quaternion.Euler(0, 0f, 0f);
        // var maxRotation = Quaternion.Euler(-160f, 0f, 0f);

        // steeringWheelTransform.localRotation = Quaternion.LerpUnclamped(minRotation, maxRotation, steering + 0.5f);
    }

    void LateUpdate()
    {
        UpdateText();
    }



    float previousAltiude;

    void UpdateText()
    {
        verticalSpeedAverage.NewValue = (envelope.transform.position.y - previousAltiude) / Time.deltaTime;

        tempText.text = "BALLOON TEMP C: " + (hotAirBalloon.BalloonTemp - 273.15f).ToString("0.0");
        altText.text = "ALT: " + envelope.position.y.ToString("0.00 M");
        verticalSpeedText.text = "VS: " + verticalSpeedAverage.MovingAverage.ToString("0.00 M/S");
        var dir = Vector3.Dot(Vector3.forward, -1 * envelope.transform.up);
        var check = Mathf.Acos(Vector3.Dot(Vector3.right, -1* envelope.transform.up));
        var HDG = Mathf.Acos(dir);
        //Debug.Log(check);
        //Debug.DrawLine(envelope.transform.position, envelope.transform.position + (dir * 1000f));
        //Debug.DrawLine(envelope.transform.position, envelope.transform.position + (envelope.transform.up * 1000f), Color.black);
        //Debug.DrawLine(envelope.transform.position, envelope.transform.position + (Vector3.forward * 10000f), Color.green);
        curHDGSpeedText.text = "HDG: ";
        if (check < Mathf.PI / 2f)
        {
            curHDGSpeedText.text += ((Mathf.Acos(dir) * Mathf.Rad2Deg)).ToString("0");
        }
        else if (check > Mathf.PI / 2f)
        {
            curHDGSpeedText.text += (360 - Mathf.Acos(dir) * Mathf.Rad2Deg).ToString("0");
        }
        var dirToTgt = "HDG->TGT: ";
        if (jobManager.Destination != null)
        {
            var envToDest = jobManager.Destination.transform.position - envelope.transform.position;
            var dirtotgtangle = Vector3.Dot(Vector3.forward, envToDest.normalized);
            var check2 = Mathf.Acos(Vector3.Dot(Vector3.right, envToDest.normalized));
            if (check2 < Mathf.PI / 2f)
            {
                //Debug.DrawLine(envelope.position, envelope.position + (envToDest*1000f), Color.red);
                dirToTgt += (Mathf.Acos(dirtotgtangle) * Mathf.Rad2Deg).ToString("0");
            }
            else if (check2 > Mathf.PI / 2)
            {
                //Debug.DrawLine(envelope.position, envelope.position + (envToDest*1000f), Color.red);
                dirToTgt += (360 - Mathf.Acos(dirtotgtangle) * Mathf.Rad2Deg).ToString("0");
            }
        }
        else
        {
            dirToTgt += " NO TGT";
        }

        destHDGSpeedText.text = dirToTgt;

        fuelText.text = "FUEL: " + (FuelInUse.Fuel * 100f).ToString("0.0") + "%";

        previousAltiude = envelope.transform.position.y;
    }
}
