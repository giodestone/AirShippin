using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// For notifying the <see cref="AirshipCockpit"/> when a button is clicked.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class AirshipButtonInteractable : Interactable
{
    public override InteractableType InteractableType => InteractableType.OneClick;

    AirshipCockpit airshipCockpit;

    [SerializeField] AirshipButtonAction buttonActionPress;
    [SerializeField] AirshipButtonAction buttonActionRelease;
    [SerializeField] string buttonAxisName;

    [SerializeField] bool enableButtonAxisControl = false;

    public bool IsButtonAxisControlEnabled { get => enableButtonAxisControl; set => enableButtonAxisControl = value; }

    ButtonPressState state;

    AudioSource audioSource;

    [SerializeField] List<AudioClip> buttonPressAudioClips;

    new void Start()
    {
        base.Start();

        airshipCockpit = GameObject.FindObjectOfType<AirshipCockpit>();

        state = ButtonPressState.Up;

        audioSource = GetComponent<AudioSource>();
    }

    // TODO: THESE MUST BE TESTED! (18:03 14/07/2021)

    public override void InteractBegin()
    {
        if (state == ButtonPressState.Down) // Do not do double press event.
            return;

        base.InteractBegin();

        PlayRandomClickSound();

        airshipCockpit.NotifyButtonPressStart(buttonActionPress);

        state = ButtonPressState.Down;
    }

    public override void InteractEnd()
    {
        if (state == ButtonPressState.Up) // Do not do double release event.
            return;
        
        base.InteractEnd();

        PlayRandomClickSound();

        airshipCockpit.NotifyButtonPressEnd(buttonActionRelease);

        state = ButtonPressState.Up;
    }

    void Update()
    {
        if (buttonAxisName.Length > 0 || !enableButtonAxisControl)
            return;
        
        if (Input.GetButtonDown(buttonAxisName))
            InteractBegin();
        else if (Input.GetButtonUp(buttonAxisName))
            InteractEnd();

    }

    void PlayRandomClickSound()
    {
        if (buttonPressAudioClips == null || buttonPressAudioClips.Count == 0)
            return;
        audioSource.PlayOneShot(buttonPressAudioClips[Random.Range(0, buttonPressAudioClips.Count)]);
    }
}
