using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelCanisterItemInteractable : ItemInteractable
{
    float fuel;
    
    /// <summary>
    /// How much fuel is in the canister.
    /// </summary>
    /// <value>Between 0 (empty), 1 (full).</value>
    
    public float Fuel {
        get { return fuel; }
        set { fuel = Mathf.Clamp01(value); }
    }
    
    public bool IsEmpty { get => fuel <= 0.0001f; }
}
