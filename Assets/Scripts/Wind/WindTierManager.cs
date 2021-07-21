using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindTierManager : MonoBehaviour
{
    const int numWindTiers = 6;
    const float windTeirAltitudeIncrement = 200f;
    const float windTierAltitudeOverlap = 40f; // Around which altitude should the wind from the upper/lower altitude be interpolated.

    List<WindTier> windTiers;
    Dictionary<float, WindTier> windBuckets; // indexed by the start altitude.

    void Start()
    {
        SetupWindTiers();
    }

    void OnEnable()
    {
        // So hot-reload works.
        if (windTiers == null || windTiers?.Count != numWindTiers)
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
        // var temp2 = GetWind(new Vector3(0f, 180f, 0f));
        // var temp21 = GetWind(new Vector3(0f, 190f, 0f));
        // var temp22 = GetWind(new Vector3(0f, 200f, 0f));
        // var temp23 = GetWind(new Vector3(0f, 210f, 0f));
        // var temp24 = GetWind(new Vector3(0f, 220f, 0f));

        // var temp4 = GetWind(new Vector3(0f, -100f, 0f));
        // var temp41 = GetWind(new Vector3(0f, -10f, 0f));
        // var temp5 = GetWind(new Vector3(0f, windTeirAltitudeIncrement * (numWindTiers + 2), 0f));

        // var temp6 = "end";
    }

    public Vector3 GetWind(Vector3 position)
    {
        // Get current bucket.
        var windBucketIndex = Mathf.Floor(position.y / windTeirAltitudeIncrement) * windTeirAltitudeIncrement;
        
        if (position.y > 0f && position.y < windTeirAltitudeIncrement * numWindTiers)
        {
            var nextBucketIndex = windBucketIndex + windTeirAltitudeIncrement;
            var prevBucketIndex = windBucketIndex - windTeirAltitudeIncrement;

            if (prevBucketIndex < 0f)
                prevBucketIndex = 0f;

            if (position.y - windBucketIndex >= -0.001f && position.y - windBucketIndex <= windTierAltitudeOverlap + 0.001f)
            {
                // Interpolate with below.
                var progressBetweenAlt = ((position.y - windBucketIndex) + windTierAltitudeOverlap) / (windTierAltitudeOverlap * 2f);

                var directionInterpolated = Vector3.Lerp(windBuckets[prevBucketIndex].GetWindDirection, windBuckets[windBucketIndex].GetWindDirection, progressBetweenAlt);
                var strengthInterplated = Mathf.Lerp(windBuckets[prevBucketIndex].GetWindStrength, windBuckets[windBucketIndex].GetWindStrength, progressBetweenAlt);
                
                return directionInterpolated * strengthInterplated;
            }
            else if (position.y - nextBucketIndex <= 0.001f && position.y - nextBucketIndex >= -windTierAltitudeOverlap - 0.001f)
            {
                // Interpolate with above.
                var progressBetweenAlt = (Mathf.Abs(position.y - nextBucketIndex) + windTierAltitudeOverlap) / (windTierAltitudeOverlap * 2f);
                progressBetweenAlt -= 1f; // Needs to be 0.0 at min, 0.5 at max.
                progressBetweenAlt = Mathf.Abs(progressBetweenAlt); // TODO reverse order.

                var directionInterpolated = Vector3.Lerp(windBuckets[windBucketIndex].GetWindDirection, windBuckets[nextBucketIndex].GetWindDirection, progressBetweenAlt);
                var strengthInterplated = Mathf.Lerp(windBuckets[windBucketIndex].GetWindStrength, windBuckets[nextBucketIndex].GetWindStrength, progressBetweenAlt);

                return directionInterpolated * strengthInterplated;
            }
            else
            {
                return windBuckets[windBucketIndex].GetWindDirection * windBuckets[windBucketIndex].GetWindStrength;
            }
        }

        var windBucketIndexClamped = Mathf.Clamp(windBucketIndex, 0f, windTeirAltitudeIncrement * (numWindTiers - 1));

        return windBuckets[windBucketIndexClamped].GetWindDirection * windBuckets[windBucketIndexClamped].GetWindStrength;
    }
}
