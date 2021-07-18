using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JobManager : MonoBehaviour
{
    JobState state;
    public JobState State 
    { 
        get 
        {
            return state;

        } 
        set
        {
            state = value;
            NotifyHubsOfStateChange();
        } 
    }

    [Header("These do not have to manually set.")]
    [SerializeField] List<ParcelHub> parcelHubs; // Set these manally to get the parcel hubs.

    ParcelHub destination;
    /// <summary>
    /// Get the current destination.
    /// </summary>
    /// <value>Will be null if <see cref="State"/> != HasJob.</value>
    public ParcelHub Destination 
    { 
        get 
        { 
            if (State == JobState.HasJob)
            {
                return destination;
            }
            return null;
        } 
    }

    List<Parcel> parcels;

    // Start is called before the first frame update
    void Start()
    {
        State = JobState.NoJob;
        parcels = new List<Parcel>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RegisterParcelHub(ParcelHub parcelHub)
    {
        if (!parcelHubs.Contains(parcelHub))
            parcelHubs.Add(parcelHub);
    }

    public void ButtonPressed(ParcelHub parcelHub, JobButtonAction jobButtonAction)
    {
        switch (State)
        {
            case JobState.NoJob:
                if (jobButtonAction == JobButtonAction.StartJob)
                    StartJob(parcelHub);
                break;
            case JobState.HasJob:
                if (jobButtonAction == JobButtonAction.CancelJob)
                    CancelCurrentJob();
                break;

        }
    }

    void StartJob(ParcelHub startHub)
    {
        var destinationHub = GenerateDestinationHub(startHub);
        destination = destinationHub;
        startHub.StartSpawningParcels(destinationHub);

        State = JobState.HasJob;
    }

    void CancelCurrentJob()
    {
        State = JobState.CancelJob; // Yes this is important, so the ParcelHub receives an update.
        RemoveAllParcels();
        State = JobState.NoJob;
    }

    ParcelHub GenerateDestinationHub(ParcelHub startHub)
    {
        var foundParcelHub = startHub;
        var iterations = 0;
        while (foundParcelHub == startHub && iterations < 100)
        {
            foundParcelHub = parcelHubs[Random.Range(0, parcelHubs.Count)];
            iterations++;
        }

        if (foundParcelHub == startHub)
            throw new System.Exception("Unable to find any start hubs that aren't the current one.");

        return foundParcelHub;
    }

    public void RegisterParcel(Parcel p)
    {
        if (!parcels.Contains(p))
        {
            parcels.Add(p);
        }
    }

    void NotifyHubsOfStateChange()
    {
        if (parcelHubs == null)
            return;
        
        foreach (var ph in parcelHubs)
        {
            ph?.NotifyOfNewJobState(State);
        }
    }

    void RemoveAllParcels()
    {
        if (parcels == null) // ok its final day don't want the game to break or soemthing.
            return;

        foreach (var p in parcels)
        {
            if (p == null)
                continue;

            Destroy(p.gameObject);
        }
    }
}
