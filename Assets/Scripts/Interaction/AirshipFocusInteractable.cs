using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implements logic when focused on the airship cockpit.
/// </summary>
public class AirshipFocusInteractable : FocusInteractable
{
    const float clickReach = 5f;
    const float throttleIncreaseSpeedModifier = 1f;
    const float steeringSpeedModifier = 1f;

    bool isFocused = false;

    AirshipCockpit airshipCockpit;

    Interactable currentInteractable;
    Interactable previousInteractable;

    new void Start()
    {
        base.Start();

        airshipCockpit = GameObject.FindObjectOfType<AirshipCockpit>();
    }

    public override void InteractBegin()
    {
        base.InteractBegin();

        isFocused = true;
    }

    public override void InteractEnd()
    {
        base.InteractEnd();

        isFocused = false;
    }

    void Update()
    {
        if (!isFocused)
            return;

        DoClickPolling();
        UpdateSteering();
        UpdateThrottle();
    }

    void DoClickPolling()
    {
        if (Input.GetButton("Fire1"))
        {
            if (Physics.Raycast(FocusCamera.transform.position, FocusCamera.transform.forward, out var hitInfo, clickReach))
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

    void UpdateThrottle()
    {
        airshipCockpit.UpdateThrottle(Input.GetAxis("Airship Throttle") * throttleIncreaseSpeedModifier * Time.deltaTime);
    }

    void UpdateSteering()
    {
        airshipCockpit.UpdateSteering(Input.GetAxis("Horizontal") * steeringSpeedModifier * Time.deltaTime);
    }
}
