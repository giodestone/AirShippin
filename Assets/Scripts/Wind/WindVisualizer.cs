using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindVisualizer : MonoBehaviour
{
    [SerializeField] GameObject trailPrefab;

    WindManager windManager;
    GameObject player;
    const string playerTag = "Player";


    const int maxNumTrails = 40;
    const float trailSpawningRadius = 50f;
    const float minTimeBeforeSpawingTrail = 0.5f;
    const float maxTimeBeforeSpawningTrail = 2.5f;

    float nextTimeToSpawnTrail = 0f;
    float timeSpawnedTrail = 0f;

    List<GameObject> trails;

    void Start()
    {
        trails = new List<GameObject>(maxNumTrails);
        windManager = GameObject.FindObjectOfType<WindManager>();
        player = GameObject.FindWithTag(playerTag);
    }

    void Update()
    {
        if (Time.time >= nextTimeToSpawnTrail)
        {
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
        if (trails.Count > maxNumTrails)
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
}
