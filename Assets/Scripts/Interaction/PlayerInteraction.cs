using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for player interacting with things.
/// </summary>
public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] new Camera camera;
    [SerializeField] Transform itemAttachmentPoint; // Where picked up items will go.
    [SerializeField] float throwItemForce = 10;
    [SerializeField] float dontThrowOverAngle = 160; // When will the player be considered to be looking at the ground. Dont throw the item if practically looking at ground.

    const float playerReach = 2f;

    PlayerInteractionState playerInteractionState;

    Interactable currentInteractable;
    Interactable previousInteractable;

    Transform originalItemParent;
    Vector3 originalItemPosition;
    Quaternion originalItemRotation;
    Vector3 originalItemScale;

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
        if (Input.GetButton("Fire1"))
        {
            if (Physics.Raycast(camera.transform.position, camera.transform.forward, out var hitInfo, playerReach))
            {
                if (InteractableDatabase.GetInteractableIfPresent(hitInfo.collider.gameObject, out var interactable))
                {
                    switch (interactable.InteractableType)
                    {
                        case InteractableType.OneClick:
                            if (currentInteractable != interactable && currentInteractable != null)
                                currentInteractable.InteractEnd();
                            else if (currentInteractable != previousInteractable && currentInteractable != interactable)
                                interactable.InteractBegin();

                            previousInteractable = currentInteractable;
                            currentInteractable = interactable;
                            break;

                        case InteractableType.Item:
                            PickUpItem(interactable);
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
        else if (currentInteractable != null)
        {
            currentInteractable.InteractEnd();
            previousInteractable = currentInteractable;
            currentInteractable = null;
        }
    }

    void HoldingItemUpdate()
    {
        currentInteractable.transform.position = itemAttachmentPoint.transform.position;

        if (Input.GetButtonDown("Fire1"))
        {
            PutDownItem();
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            ThrowOrPutDownItem();
        }
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

    void PickUpItem(Interactable interactable)
    {
        currentInteractable = interactable;
        originalItemParent = currentInteractable.transform.parent;
        originalItemPosition = currentInteractable.transform.position;
        originalItemRotation = currentInteractable.transform.rotation;
        originalItemScale = currentInteractable.transform.localScale;
        currentInteractable.transform.localPosition = Vector3.zero;

        currentInteractable.gameObject.transform.parent = itemAttachmentPoint;
        currentInteractable.transform.gameObject.GetComponent<Collider>().enabled = false;
        currentInteractable.transform.gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }

    void PutDownItem()
    {
        currentInteractable.transform.gameObject.GetComponent<Collider>().enabled = true;

        if (Physics.Raycast(itemAttachmentPoint.position, -itemAttachmentPoint.transform.up, out var hitInfo, 5f, LayerMask.GetMask("ItemPutDownSurface")))
        {
            // Check if raycast hit an ItemHolder
            var itemHolder = hitInfo.transform.gameObject.GetComponent<ItemHolder>();
            if (itemHolder != null)
            {
                throw new NotImplementedException();
            }
            // Otherwise plonk it down infront of player and set it as the parent transform so it doesn't flop about.
            else
            {
                currentInteractable.transform.parent = hitInfo.transform;
                currentInteractable.transform.position = hitInfo.point;
                currentInteractable.transform.rotation = Quaternion.Euler(hitInfo.normal);
                currentInteractable.transform.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            }

        }
        // Otherwise let physics take the wheel.
        else
        {
            currentInteractable.transform.parent = null;
            currentInteractable.transform.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }

        currentInteractable = null;
        SwitchStateTo(PlayerInteractionState.Free);
    }

    void ThrowOrPutDownItem()
    {
        currentInteractable.transform.parent = null;
        currentInteractable.transform.gameObject.GetComponent<Rigidbody>().isKinematic = false;

        currentInteractable.transform.gameObject.GetComponent<Collider>().enabled = true;

        if (Vector3.Angle(Vector3.up, camera.transform.up) < dontThrowOverAngle)
        {
            currentInteractable.transform.gameObject.GetComponent<Rigidbody>()?.AddForce(camera.transform.forward * throwItemForce, ForceMode.Impulse);
        }

        currentInteractable = null;
        SwitchStateTo(PlayerInteractionState.Free);
    }
}
