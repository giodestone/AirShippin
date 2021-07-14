using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// For notifying the <see cref="AirshipCockpit"/> when a button is clicked.
/// </summary>
public class AirshipButtonInteractable : Interactable
{
    public override InteractableType InteractableType => InteractableType.OneClick;

    AirshipCockpit airshipCockpit;

    [SerializeField] AirshipButtonAction buttonAction;

    new void Start()
    {
        base.Start();

        airshipCockpit = GameObject.FindObjectOfType<AirshipCockpit>();
    }

    public override void InteractBegin()
    {
        base.InteractBegin();

        airshipCockpit.NotifyButtonPressStart(buttonAction);
    }

    public override void InteractEnd()
    {
        base.InteractEnd();

        airshipCockpit.NotifyButtonPressEnd(buttonAction);
    }
}
