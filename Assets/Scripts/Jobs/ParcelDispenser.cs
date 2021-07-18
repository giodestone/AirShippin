using UnityEngine;

public class ParcelDispenser : ItemHolderWithItemSpawner
{
    public ParcelHub ParcelDestination { get; set; }

    protected override void OnItemSpawned(GameObject obj)
    {
        base.OnItemSpawned(obj);

        SetParcelDestination(obj);
    }

    void SetParcelDestination(GameObject objContainingParcel)
    {
        objContainingParcel.GetComponent<Parcel>().Destination = ParcelDestination;
    }
}