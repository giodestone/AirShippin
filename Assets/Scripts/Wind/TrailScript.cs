using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailScript : MonoBehaviour
{
    WindManager windManager;
    WindVisualizer windVisualizer;

    public void Initialise(WindManager windManager, WindVisualizer windVisualizer)
    {
        this.windManager = windManager;
        this.windVisualizer = windVisualizer;
    }

    void Start()
    {
        Destroy(this.gameObject, GetComponent<TrailRenderer>().time);
    }

    void OnDestroy()
    {
        windVisualizer.TrailDestroyed(gameObject);
    }

    void LateUpdate()
    {
        var wind = windManager.GetWind(transform.position);

        transform.position = new Vector3(
            transform.position.x + (wind.x * Time.deltaTime), // Wind is in ms-1
            transform.position.y + (wind.y * Time.deltaTime),
            transform.position.z + (wind.z * Time.deltaTime)
        );
    }
}
