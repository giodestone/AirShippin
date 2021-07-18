using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class from which to derive interactable behaviour from
/// </summary>
public class Interactable : MonoBehaviour
{
    [Header("Chaning this wont do anything (only for debug view).")]
    [SerializeField] InteractableType interactableType;

    /// <summary>
    /// Get what type of interactable this is.
    /// </summary>
    /// <value></value>
    public virtual InteractableType InteractableType { get => InteractableType.Undefined; }

    private ItemHolder owningItemHolder;

    /// <summary>
    /// Which <see cref="ItemHolder"/> 'owns' this item currently. InteractableType must be InteractableType.Item.
    /// </summary>
    /// <value>null if none, a value if so.</value>
    public ItemHolder OwningItemHolder 
    { 
        get
        {
            if (InteractableType != InteractableType.Item)
                throw new System.Exception("This object cannot have an owning ItemHolder due to its InteractableType.");
            
            return owningItemHolder;
        }
        set
        {
            if (InteractableType != InteractableType.Item)
                throw new System.Exception("This object cannot have an owning ItemHolder due to its InteractableType.");

            owningItemHolder = value;
        }
    }

    protected void Start()
    {
        interactableType = InteractableType;
        InteractableDatabase.RegisterInteractable(this);
    }

    void OnDestroy()
    {
        InteractableDatabase.UnregiserInteractable(this);
        Debug.Log("parent");
        OnDestoryOverridable();
    }

    /// <summary>
    /// Override this if you wanna add functionality for destorying becuase OnDestroy only is called by the derived class!
    /// </summary>    
    protected virtual void OnDestoryOverridable()
    {

    }

    /// <summary>
    /// Instruct that an interaction has begin (player has clicked on it).
    /// </summary>
    public virtual void InteractBegin()
    {

    }

    /// <summary>
    /// Instruct that an interaction has stopped (player has stopped clicking on it).
    /// </summary>
    public virtual void InteractEnd()
    {

    }
}
