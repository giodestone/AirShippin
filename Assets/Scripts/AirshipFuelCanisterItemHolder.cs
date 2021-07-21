using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirshipFuelCanisterItemHolder : ItemHolder
{
    FuelCanisterItemInteractable currentInteractableFuelCansiter;

    /// <summary>
    /// Get the amount of fuel in the canister.
    /// </summary>
    /// <value>0 if item is not canister or no canister in holder; the amount of fuel in the canister if present. See: <see cref="FuelCanisterItemInteractable.Fuel"/>.</value>
    public float Fuel { 
        get 
        {
            if (IsHolderFull)
            {
                if (currentInteractableFuelCansiter != null)
                {
                    return currentInteractableFuelCansiter.Fuel;
                }
            }
            return 0f;
        }
        set
        {
            if (IsHolderFull)
            {
                if (currentInteractableFuelCansiter != null)
                {
                    currentInteractableFuelCansiter.Fuel = value;
                }
            }
        }
    }

    public override void PutItemIn(GameObject item)
    {
        currentInteractableFuelCansiter = item.GetComponent<FuelCanisterItemInteractable>();
        if (currentInteractableFuelCansiter == null)
            currentInteractableFuelCansiter = item.GetComponentInChildren<FuelCanisterItemInteractable>();
        
        base.PutItemIn(item);
    }

    public override void TakeItemOut(GameObject item)
    {
        currentInteractableFuelCansiter = null;

        base.TakeItemOut(item);
    }
}
