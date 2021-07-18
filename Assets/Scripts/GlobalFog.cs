using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class GlobalFog : MonoBehaviour
{

    Volume volume;
    public bool enableFog;
    public bool overrideFog;
    public Fog fog;
    private float maxFreePath = 5000f;
    

// Start is called before the first frame update
void Start()
    {
        VolumeProfile profile = GetComponent<Volume>().profile;
        if (profile.TryGet<Fog>(out var fog))
        {
            this.fog = fog;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(AtmosphereManager.pollution);
        Debug.Log(this.fog.meanFreePath.min);
        this.fog.meanFreePath.min = maxFreePath - AtmosphereManager.pollution * 10f;
        this.fog.meanFreePath.min = Mathf.Clamp(this.fog.meanFreePath.min, 0f, float.PositiveInfinity);
    }
}
