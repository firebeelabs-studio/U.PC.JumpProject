using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivatePlayerTemp : MonoBehaviour
{
    public GameObject objToSetActive;
    void Start()
    {
        if (SceneManager.GetActiveScene () == SceneManager.GetSceneByName ("strykusLevel2"))
        {
            objToSetActive.SetActive(true);
        }
    }
}
