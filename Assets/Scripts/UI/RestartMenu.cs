using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartMenu : MonoBehaviour
{
    [SerializeField] GameObject restartMenu;
    [SerializeField] float pressAndHoldTime = 5f;
    [SerializeField] string sceneToLoadName;

    float timeButtonPressed;

    bool isDisplayingRestartMenu = false;
    bool wasButtonDown = false;
    bool enableCounter = false;

    void Update()
    {
        if (isDisplayingRestartMenu) // This should be a state machine.
        {
            if (Input.GetButtonDown("Pause") && !wasButtonDown)
            {
                isDisplayingRestartMenu = false;
                Cursor.lockState = CursorLockMode.Locked;
                wasButtonDown = true;
            }
        }
        else
        {
            if (Input.GetButtonDown("Pause") && !wasButtonDown)
            {
                wasButtonDown = true;
                timeButtonPressed = 0f;
                enableCounter = true;
            }

            if (Input.GetButton("Pause") && enableCounter)
            {
                timeButtonPressed += Time.deltaTime;
                
                if (timeButtonPressed >= pressAndHoldTime)
                {
                    Cursor.lockState = CursorLockMode.None;
                    isDisplayingRestartMenu = true;
                    enableCounter = false;
                }
            }
        }

        if (Input.GetButtonUp("Pause"))
        {
            wasButtonDown = false;
            enableCounter = false;
        }

        restartMenu.SetActive(isDisplayingRestartMenu);
    }

    public void OnRestartButtonPressed()
    {
        SceneManager.LoadScene(sceneToLoadName, LoadSceneMode.Single);
    }
}
