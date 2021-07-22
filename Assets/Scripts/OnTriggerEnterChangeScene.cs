using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnTriggerEnterChangeScene : MonoBehaviour
{
    [SerializeField] string nameOfScene;
    [SerializeField] Collider playerCollider;

    void OnTriggerEnter(Collider other)
    {
        if (other == playerCollider)
            SceneManager.LoadScene(nameOfScene);
    }
}
