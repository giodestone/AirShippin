using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteractable : Interactable
{
    public override InteractableType InteractableType { get => InteractableType.Item; }

    public ItemHolder ItemHolder { get; set; }
    public bool IsInHolder { get => ItemHolder != null; }

    public override void InteractBegin()
    {
        base.InteractBegin();

        if (IsInHolder)
        {
            ItemHolder.TakeItemOut(this.gameObject);
        }
    }

    public override void InteractEnd()
    {
        base.InteractEnd();
    }

    protected override void OnDestoryOverridable()
    {
        base.OnDestoryOverridable();
        
        ItemHolder?.ItemHeldDestroyed();
    }
}
