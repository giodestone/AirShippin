using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailScript : MonoBehaviour
{
    WindManager windManager;
    WindVisualizer windVisualizer;

    float lifetime;
    float timeCreated;

    const float speedMultiplier = 2f;

    public void Initialise(WindManager windManager, WindVisualizer windVisualizer)
    {
        this.windManager = windManager;
        this.windVisualizer = windVisualizer;
    }

    void Start()
    {
        lifetime = GetComponent<TrailRenderer>().time;
        timeCreated = Time.time;
    }

    void OnDestroy()
    {
        windVisualizer.TrailDestroyed(gameObject);
    }

    void LateUpdate()
    {
        if (Time.time >= timeCreated + lifetime)
            return;
            
        var wind = windManager.GetWind(transform.position);

        transform.position = new Vector3(
            transform.position.x + (wind.x * Time.deltaTime * speedMultiplier), // Wind is in ms-1
            transform.position.y + (wind.y * Time.deltaTime * speedMultiplier),
            transform.position.z + (wind.z * Time.deltaTime * speedMultiplier)
        );
    }
}
