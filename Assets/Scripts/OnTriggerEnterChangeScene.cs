using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnTriggerEnterChangeScene : MonoBehaviour
{
    [SerializeField] string nameOfScene;

    void OnTriggerEnter()
    {
        SceneManager.LoadScene(nameOfScene);
    }
}
