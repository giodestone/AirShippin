using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindTierManager : MonoBehaviour
{
    const float numWindTiers = 6;
    const float windTeirAltitudeIncrement = 200f;

    List<WindTier> windTiers;
    Dictionary<float, WindTier> windBuckets;

    void Start()
    {
        SetupWindTiers();
    }

    void SetupWindTiers()
    {
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
    }

    Vector3 GetWind(Vector3 position)
    {
        // TODO actually verify that this works.
        var windBucketIndex = (float)System.Math.Round(position.y / windTeirAltitudeIncrement, 0) * windTeirAltitudeIncrement;
        var wt = windBuckets[windBucketIndex];

        return wt.GetWindDirection * wt.GetWindStrength;
    }
}
