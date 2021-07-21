using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneOnClick : MonoBehaviour
{
    [SerializeField] string nameOfScene;

    public void LoadScene()
    {
        SceneManager.LoadScene(nameOfScene);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(nameOfScene));
    }
}
