using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parcel : ItemInteractable
{
    ParcelHub destination;
    public ParcelHub Destination { get => destination; set { destination = value; OnDestinationSet(); } }


    void OnDestinationSet()
    {
        destination.JobManager.RegisterParcel(this);
    }
}
