using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteractable : Interactable
{
    public override InteractableType InteractableType { get => InteractableType.Item; }

    public override void InteractBegin()
    {
        base.InteractBegin();
    }

    public override void InteractEnd()
    {
        base.InteractEnd();
    }
}
