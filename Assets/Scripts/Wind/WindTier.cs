using UnityEngine;

[System.Serializable]
public class WindTier{
    public const float MinWindStrength = 0f;
    public const float MaxWindStrength = 10f;
    public const float MinWindDirectionTransitionSeconds = 60f;
    public const float MaxWindDirectionTransitionSeconds = 60f * 3f;

    public const float MinWindStrengthTransitionSeconds = MinWindDirectionTransitionSeconds;
    public const float MaxWindStrengthTransitionSeconds = MaxWindDirectionTransitionSeconds;

    public float AltitudeStart { get; set; }
    public float AltitudeEnd { get; set; }
    public bool IsTopTierAlt { get; set; } // Because then the wind should just have a down current.

    public Vector3 StartWindDirection { get; set; }
    public Vector3 TargetWindDirection { get; set; }
    public float WindDirectionTransitionProgress { get => Mathf.Clamp01(windDirectionTransitionElapsedSeconds / WindDirectionTransitionSeconds); }

    float windDirectionTransitionElapsedSeconds;
    public float WindDirectionTransitionSeconds { get; set; }

    public float StartWindStrength { get; set; }
    public float TargetWindStrength { get; set; }
    public float WindStrengthTransitionProgress { get => Mathf.Clamp01(windStrengthTransitionSecondsElapsed / WindStrengthTransitionSeconds); }

    float windStrengthTransitionSecondsElapsed;
    public float WindStrengthTransitionSeconds { get; set; }

    /// <summary>
    /// Get the interpolated wind direction as a unit vector.
    /// </summary>
    /// <returns></returns>
    public Vector3 GetWindDirection { get => Vector3.Lerp(StartWindDirection, TargetWindDirection, WindDirectionTransitionProgress).normalized;  }

    /// <summary>
    /// Get the wind strength.
    /// </summary>
    /// <returns></returns>
    public float GetWindStrength { get => Mathf.Lerp(StartWindStrength, TargetWindStrength, WindStrengthTransitionProgress); }

    public void Update(float deltaTime)
    {
        windStrengthTransitionSecondsElapsed += deltaTime;
        windDirectionTransitionElapsedSeconds += deltaTime;

        if (WindStrengthTransitionProgress >= 0.9999f)
            GenerateNextWindStrengthStats();
        
        if (WindDirectionTransitionProgress >= 0.9999f)
            GenerateNextWindDirectionStats();
    }

    void GenerateNextWindStrengthStats()
    {
        StartWindDirection = TargetWindDirection;
        StartWindStrength = TargetWindStrength;

        TargetWindStrength = Random.Range(MinWindStrength, MaxWindStrength);

        windStrengthTransitionSecondsElapsed = 0f;
        WindStrengthTransitionSeconds = Random.Range(MinWindStrengthTransitionSeconds, MaxWindStrengthTransitionSeconds);
    }

    void GenerateNextWindDirectionStats(float yOverride=0.5f)
    {
        TargetWindDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        if (IsTopTierAlt)
            TargetWindDirection = new Vector3(TargetWindDirection.x, yOverride, TargetWindDirection.z);

        TargetWindDirection.Normalize();

        windDirectionTransitionElapsedSeconds = 0f;
        WindDirectionTransitionSeconds = Random.Range(MinWindDirectionTransitionSeconds, MaxWindDirectionTransitionSeconds);
    }
}