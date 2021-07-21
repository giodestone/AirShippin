using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindVisualizer : MonoBehaviour
{
    [SerializeField] GameObject trailPrefab;

    WindManager windManager;
    GameObject player;
    const string playerTag = "Player";

    const int maxNumTrails = 60;
    const float trailSpawningRadius = 300f;
    const float minTimeBeforeSpawingTrail = 0.25f;
    const float maxTimeBeforeSpawningTrail = 1.5f;

    int numTrailsInRadius;
    float nextTimeToSpawnTrail = 0f;
    float timeSpawnedTrail = 0f;

    List<GameObject> trails;
    Dictionary<float, GameObject> distanceToTrail;

    void Start()
    {
        trails = new List<GameObject>(maxNumTrails);
        distanceToTrail = new Dictionary<float, GameObject>(maxNumTrails);
        windManager = GameObject.FindObjectOfType<WindManager>();
        player = GameObject.FindWithTag(playerTag);
    }

    void Update()
    {
        if (Time.time >= nextTimeToSpawnTrail)
        {
            UpdateTrailsInRadius();
            CalculateNextTimeToSpawnTrail();
            SpawnTrail();
        }
    }

    void CalculateNextTimeToSpawnTrail()
    {
        nextTimeToSpawnTrail = Time.time + Random.Range(minTimeBeforeSpawingTrail, maxTimeBeforeSpawningTrail);
    }

    void SpawnTrail()
    {
        if (numTrailsInRadius > maxNumTrails)
            return;
        
        var newTrail = GameObject.Instantiate(trailPrefab, PickPosition(), Quaternion.identity);
        trails.Add(newTrail);
        newTrail.transform.parent = this.transform;
        var newTrailScript = newTrail.GetComponent<TrailScript>();
        newTrailScript.Initialise(windManager, this);
    }

    Vector3 PickPosition()
    {
        return new Vector3(
            Random.Range(-trailSpawningRadius, trailSpawningRadius) + player.transform.position.x,
            Random.Range(-trailSpawningRadius, trailSpawningRadius) + player.transform.position.y,
            Random.Range(-trailSpawningRadius, trailSpawningRadius) + player.transform.position.z
        );
    }

    public void TrailDestroyed(GameObject trail)
    {
        trails.Remove(trail);
    }

    /// <summary>
    /// Update the variable <see cref="numTrailsInRadius"/>.
    /// </summary>
    /// <param name="extraDistanceToleranceMultiplier">Multiplier of <see cref="trailSpawningRadius"/> for which trails will get excluded from being considered in radius, but won't be destroyed.</param>
    void UpdateTrailsInRadius(float extraDistanceToleranceMultiplier=2f)
    {
        distanceToTrail.Clear();

        // Thinking about it some max processing time would be in order.

        numTrailsInRadius = trails.Count;

        foreach (var trail in trails)
        {
            var sqrDistance = (trail.transform.position - player.transform.position).sqrMagnitude;
            if (sqrDistance > Mathf.Pow(trailSpawningRadius, 2f))
                numTrailsInRadius--;
        
            if (sqrDistance > Mathf.Pow(trailSpawningRadius * extraDistanceToleranceMultiplier, 2f))
                Destroy(trail);
        }
    }
}
