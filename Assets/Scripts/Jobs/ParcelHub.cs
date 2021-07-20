using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ParcelHub : MonoBehaviour
{
    [SerializeField] new string name;
    [SerializeField] ParcelDispenser itemSpawner;
    [SerializeField] ParcelDeliveryPlatform parcelDeliveryPlatform;
    [SerializeField] FireworkController fireworkController;

    [SerializeField] TextMeshProUGUI parcelHubNameText;
    [SerializeField] TextMeshProUGUI currentJobText;

    const string preTitleAppend = "Hub Name: ";
    const string preJobAppend = "Current Job: ";

    List<JobButtonOneClickInteractable> buttons;

    JobManager jobManager;
    public JobManager JobManager { get => jobManager; }

    const int numBoxesToSpawn = 5;

    void Awake()
    {
        buttons = new List<JobButtonOneClickInteractable>();
    }

    void Start()
    {
        jobManager = GameObject.FindObjectOfType<JobManager>();
        jobManager.RegisterParcelHub(this);
        itemSpawner.ShouldSpawn = false;
        parcelHubNameText.text = preTitleAppend + name;
        NotifyOfNewJobState(JobState.NoJob);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RegisterButton(JobButtonOneClickInteractable b)
    {
        buttons.Add(b);
    }

    public void ButtonPressed(JobButtonAction buttonAction)
    {
        jobManager.ButtonPressed(this, buttonAction);
    }

    public void StartSpawningParcels(ParcelHub destinationHub)
    {
        itemSpawner.ParcelDestination = destinationHub;
        itemSpawner.ShouldSpawn = true;
        itemSpawner.NumToSpawn = numBoxesToSpawn;
    }

    public void NotifyOfNewJobState(JobState newJobState)
    {
        switch (newJobState)
        {
            case JobState.CancelJob:
                itemSpawner.ShouldSpawn = false;
                goto case JobState.NoJob;

            case JobState.SubmitJob:
                var parcelsOnPlatform = parcelDeliveryPlatform.ParcelsOnPlatfom;
                var correctParcels = 0;
                if (parcelsOnPlatform.Count > 0)
                {
                    foreach (var p in parcelsOnPlatform)
                        if (p.Destination == this)
                            correctParcels++;

                    if (correctParcels > 0)
                        fireworkController.RunFirework();
                }

                AtmosphereManager.ParcelsDelivered((float)parcelsOnPlatform.Count);
                goto case JobState.NoJob;
            
            case JobState.NoJob:
                currentJobText.text = preJobAppend + "No current job.";
                break;
            
            case JobState.HasJob:
                currentJobText.text = preJobAppend + "Deliver boxes to: " + jobManager.Destination.name;
                break;

        }
    }
}
