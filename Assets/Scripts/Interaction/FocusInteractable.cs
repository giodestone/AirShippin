using UnityEngine;

public class FocusInteractable : Interactable
{
    public override InteractableType InteractableType => InteractableType.Focus;

    Camera mainCamera;
    [SerializeField] Camera focusCamera; // Camera which will be enabled after focusing.
    [SerializeField] Rigidbody rigidBodyToFixItemTo;

    /// <summary>
    /// Get the camera which is used when it is focused.
    /// </summary>
    /// <value></value>
    protected Camera FocusCamera { get => focusCamera; }

    Vector3 playerPosFromObjectOnInteract;

    PlayerInteraction playerInteraction;

    FixedJoint createdJoint;


    void Awake()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        playerInteraction = GameObject.FindObjectOfType<PlayerInteraction>();

        focusCamera.enabled = false;
        SetFirstPersonLookEnabledState(false);
    }

    public override void InteractBegin()
    {
        base.InteractBegin();

        mainCamera.enabled = false;
        focusCamera.enabled = true;
        focusCamera.GetComponent<FirstPersonLook>().enabled = true;

        SetPlayerControlState(false);
        SetFirstPersonLookEnabledState(true);
        FixPlayerInPlace();
    }

    public override void InteractEnd()
    {
        base.InteractEnd();

        focusCamera.enabled = false;
        mainCamera.enabled = true;

        UnfixPlayer();
        SetFirstPersonLookEnabledState(false);
        SetPlayerControlState(true);
    }

    void SetPlayerControlState(bool state)
    {
        var playerGO = playerInteraction.gameObject;
        playerGO.GetComponent<FirstPersonMovement>().enabled = state;
        playerGO.GetComponent<Jump>().enabled = state;
        playerGO.GetComponentInChildren<FirstPersonLook>().enabled = state;
        playerGO.GetComponentInChildren<Zoom>().enabled = state;
    }

    /// <summary>
    /// Set the <see cref="FirstPersonLook"/> script attached to the <see cref="focus"/> enabled to state. Checks if one acctually exists.
    /// </summary>
    /// <param name="state"></param>
    void SetFirstPersonLookEnabledState(bool state)
    {
        var component = focusCamera.GetComponent<FirstPersonLook>();

        if (component != null)
            component.enabled = state;
    }

    void FixPlayerInPlace()
    {
        // var playerGO = playerInteraction.gameObject;
        // createdJoint = playerGO.AddComponent<FixedJoint>();
        // createdJoint.connectedBody = rigidBodyToFixItemTo;
    }

    void UnfixPlayer()
    {
        // Destroy(createdJoint);
        // createdJoint = null;
    }
}