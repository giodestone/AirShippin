using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Logic for a button which lets the player get a new job or interact with things.
/// </summary>
public class JobButtonOneClickInteractable : Interactable
{
    public override InteractableType InteractableType { get => InteractableType.OneClick; }

    [SerializeField] JobButtonAction jobButtonAction;
    [SerializeField] ParcelHub parcelHub;

    new void Start()
    {
        base.Start();
    }

    public override void InteractBegin()
    {
        base.InteractBegin();

        parcelHub.ButtonPressed(jobButtonAction);
    }
}
