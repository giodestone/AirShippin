using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParcelDeliveryPlatform : MonoBehaviour
{
    public List<Parcel> ParcelsOnPlatfom { get => GetParcelsOnHolder(); }

    List<Parcel> GetParcelsOnHolder()
    {
        return new List<Parcel>(GetComponentsInChildren<Parcel>());
    }
}
