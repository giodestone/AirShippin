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
        volume = GetComponent<Volume>();
        VolumeProfile profile = volume.profile;
        if (profile.TryGet<Fog>(out var fog))
        {
            this.fog = fog;
        }
        this.fog.enabled.overrideState = true;
        this.fog.enabled.value = true;
        this.fog.enabled.overrideState = false;
        this.fog.maximumHeight.overrideState = true;
        this.fog.maximumHeight.value = 5000f;
        this.fog.maximumHeight.overrideState = false;
        this.fog.meanFreePath.overrideState = true;
        this.fog.meanFreePath.value = 5000f;
    }

    // Update is called once per frame
    void Update()
    {
        this.fog.meanFreePath.value = maxFreePath - AtmosphereManager.pollution;
        this.fog.meanFreePath.value = Mathf.Clamp(this.fog.meanFreePath.value, 1f, float.PositiveInfinity);
    }
}
