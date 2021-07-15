using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindTierManager : MonoBehaviour
{
    const int numWindTiers = 6;
    const float windTeirAltitudeIncrement = 200f;
    const float windTierAltitudeOverlap = 20f; // Around which altitude should the wind from the upper/lower altitude be interpolated.

    List<WindTier> windTiers;
    Dictionary<float, WindTier> windBuckets;

    void Start()
    {
        SetupWindTiers();
    }

    void SetupWindTiers()
    {
        windTiers = new List<WindTier>(capacity: numWindTiers);
        windBuckets = new Dictionary<float, WindTier>(capacity: numWindTiers);

        for (var i = 0; i < numWindTiers; ++i)
        {
            var wt = new WindTier();
            wt.AltitudeStart = i * windTeirAltitudeIncrement;

            windBuckets.Add(i * windTeirAltitudeIncrement, wt);

            if (i == numWindTiers - 1)
                wt.AltitudeEnd = float.MaxValue;
            else
                wt.AltitudeEnd = i + 1 * windTeirAltitudeIncrement;

            wt.StartWindDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            wt.TargetWindDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));

            if (i == numWindTiers - 1)
            {
                wt.StartWindDirection = new Vector3(wt.StartWindDirection.x, -0.5f, wt.StartWindDirection.z);
                wt.TargetWindDirection = new Vector3(wt.TargetWindDirection.x, -0.5f, wt.TargetWindDirection.z);
            }
            
            wt.StartWindDirection.Normalize();
            wt.TargetWindDirection.Normalize();

            wt.WindDirectionTransitionSeconds = Random.Range(WindTier.MinWindDirectionTransitionSeconds, WindTier.MaxWindDirectionTransitionSeconds);

            wt.StartWindStrength = Random.Range(0f, WindTier.MaxWindStrength * (((float)i + 1) / (float)numWindTiers));
            wt.TargetWindStrength = Random.Range(0f, WindTier.MaxWindStrength * (((float)i + 1) / (float)numWindTiers));

            wt.WindStrengthTransitionSeconds = Random.Range(WindTier.MinWindStrengthTransitionSeconds, WindTier.MaxWindStrengthTransitionSeconds);
        }
    }

    void UpdateWindTiers()
    {
        foreach (var windTier in windTiers)
        {
            windTier.Update(Time.deltaTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateWindTiers();

        //var temp = GetWind(Vector3.zero);
        var temp2 = GetWind(new Vector3(0f, 180f, 0f));
        var temp21 = GetWind(new Vector3(0f, 190f, 0f));
        var temp22 = GetWind(new Vector3(0f, 200f, 0f));
        var temp23 = GetWind(new Vector3(0f, 210f, 0f));
        var temp24 = GetWind(new Vector3(0f, 220f, 0f));

        var temp4 = GetWind(new Vector3(0f, -100f, 0f));
        var temp41 = GetWind(new Vector3(0f, -10f, 0f));
        var temp5 = GetWind(new Vector3(0f, windTeirAltitudeIncrement * (numWindTiers + 2), 0f));

        var temp6 = "end";
    }

    Vector3 GetWind(Vector3 position)
    {
        var windBucketIndex = (float)System.Math.Round(position.y / windTeirAltitudeIncrement, 0) * windTeirAltitudeIncrement;
        var windBucketIndexClamped = Mathf.Clamp(windBucketIndex, 0f, windTeirAltitudeIncrement * (numWindTiers - 1));

        var nextWindBucket = windBucketIndex + windTeirAltitudeIncrement;
        var prevWindBucket = windBucketIndex - windTeirAltitudeIncrement;

        if (position.y > 0f && position.y <= numWindTiers * windTeirAltitudeIncrement)
        {
            var distToUpper = 

            // Check Below
            if (position.y - windBucketIndex >= -0.001f && position.y - windBucketIndex <= windTierAltitudeOverlap + 0.001f)
            {
                // Interpolate with below.
                var progressBetweenAlt = ((position.y - windBucketIndex)) / (windTierAltitudeOverlap * 2f);

                var directionInterpolated = Vector3.Lerp(windBuckets[prevWindBucket].GetWindDirection, windBuckets[windBucketIndex].GetWindDirection, progressBetweenAlt);
                var strengthInterplated = Mathf.Lerp(windBuckets[prevWindBucket].GetWindStrength, windBuckets[windBucketIndex].GetWindStrength, progressBetweenAlt);
                
                return directionInterpolated * strengthInterplated;
            }
            else if (position.y - windBucketIndex <= 0.001f && position.y - windBucketIndex >= -windTierAltitudeOverlap - 0.001f)
            {
                // Interpolate with above.
                var progressBetweenAlt = (Mathf.Abs(position.y - windBucketIndex)) / (windTierAltitudeOverlap * 2f) ;

                var directionInterpolated = Vector3.Lerp(windBuckets[nextWindBucket].GetWindDirection, windBuckets[windBucketIndex].GetWindDirection, progressBetweenAlt);
                var strengthInterplated = Mathf.Lerp(windBuckets[nextWindBucket].GetWindStrength, windBuckets[windBucketIndex].GetWindStrength, progressBetweenAlt);

                return directionInterpolated * strengthInterplated;
            }
            else
            {
                throw new System.Exception();
            }
        }
        else
        {
            var wt = windBuckets[windBucketIndex];
            return wt.GetWindDirection * wt.GetWindStrength;
        }        
    }
}
