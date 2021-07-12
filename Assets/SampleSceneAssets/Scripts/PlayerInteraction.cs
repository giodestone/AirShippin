using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for player interacting with things.
/// </summary>
public class PlayerInteraction : MonoBehaviour
{
    new Camera camera;
    const float playerReach = 2f;

    PlayerInteractionState playerInteractionState;

    Interactable currentInteractable;

    void Update()
    {
        switch (playerInteractionState)
        {
            case PlayerInteractionState.Free:
                FreeStateUpdate();
                break;
            case PlayerInteractionState.HoldingItem:
                HoldingItemUpdate();
                break;
            case PlayerInteractionState.Focused:
                FocusedUpdate();
                break;
        }
    }

    void FreeStateUpdate()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (Physics.Raycast(camera.transform.position, camera.transform.forward, out var hitInfo, playerReach))
            {
                if (InteractableDatabase.GetInteractableIfPresent(hitInfo.collider.gameObject, out var interactable))
                {
                    switch (interactable.InteractableType)
                    {
                        case InteractableType.OneClick:
                            if (currentInteractable != interactable)
                                currentInteractable.InteractEnd();

                            interactable.InteractBegin();

                            currentInteractable = interactable;
                            break;

                        case InteractableType.Item:
                            throw new NotImplementedException();
                            // TODO: Pick item up and switch state.
                            SwitchStateTo(PlayerInteractionState.HoldingItem);
                            break;
                        
                        case InteractableType.Focus:
                            throw new NotImplementedException();
                            // TODO: switch to the focused item.
                            SwitchStateTo(PlayerInteractionState.Focused);
                            break;
                    }
                }
            }
        }
        else
        {
            currentInteractable.InteractEnd();
            currentInteractable = null;
        }
    }

    void HoldingItemUpdate()
    {
        // TODO put item down if Fire1 clicked.

    }

    void FocusedUpdate()
    {
        // TODO detect if cancel is pressed so then the focus can be ended.
    }

    void SwitchStateTo(PlayerInteractionState newPis)
    {
        switch (newPis)
        {
            case PlayerInteractionState.Free:
                switch (playerInteractionState)
                {
                    case PlayerInteractionState.HoldingItem:
                        // TODO drop current item
                        break;
                    case PlayerInteractionState.Focused:
                        // TODO stop interacting.
                        break;
                }
                break;
            case PlayerInteractionState.HoldingItem:
                switch (playerInteractionState)
                {
                    case PlayerInteractionState.Free:
                        // Nothing? Shouldn't be interacting with non picked up object.
                        break;
                    case PlayerInteractionState.Focused:
                        // Nothing? Picking up things in focused view may be possible, in which case TODO ?
                        break;
                }
                break;
            case PlayerInteractionState.Focused:
                switch (playerInteractionState)
                {
                    case PlayerInteractionState.Free:
                        // Nothing? currentInteractable should be the item we're tryng to focus to?
                        break;
                    case PlayerInteractionState.HoldingItem:
                        // Nothing? Shouldn't be possible to transfer from holding item into focused anyway.
                        break;
                }
                break;
        }
        playerInteractionState = newPis;
    }
}
