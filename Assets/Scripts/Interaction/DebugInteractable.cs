using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugInteractable : Interactable
{
    public override InteractableType InteractableType { get => InteractableType.OneClick; }
    public override void InteractBegin()
    {
        base.InteractBegin();
        Debug.Log("Pressed!");
    }

    public override void InteractEnd()
    {
        base.InteractEnd();
        Debug.Log("Released!");
    }
}
