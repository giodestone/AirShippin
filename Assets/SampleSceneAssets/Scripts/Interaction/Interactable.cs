using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class from which to derive interactable behaviour from
/// </summary>
public class Interactable : MonoBehaviour
{
    [SerializeField] InteractableType interactableType;

    /// <summary>
    /// Get what type of interactable this is.
    /// </summary>
    /// <value></value>
    public InteractableType InteractableType { get => interactableType; }

    void Start()
    {
        InteractableDatabase.RegisterInteractable(this);
    }

    void OnDestory()
    {
        InteractableDatabase.UnregiserInteractable(this);
    }

    /// <summary>
    /// Instruct that an interaction has begin (player has clicked on it).
    /// </summary>
    public virtual void InteractBegin()
    {

    }

    /// <summary>
    /// Instruct that an interaction has stopped (player has stopped clicking on it)
    /// </summary>
    public virtual void InteractEnd()
    {

    }

    /// <summary>
    /// Instruct that the interaction should be ended, if focused.
    /// </summary>
    public virtual void Release()
    {

    }
}
