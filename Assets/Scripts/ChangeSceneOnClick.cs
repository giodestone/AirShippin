using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneOnClick : MonoBehaviour
{
    [SerializeField] string nameOfScene;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(nameOfScene);
    }
}
