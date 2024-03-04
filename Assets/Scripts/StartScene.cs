using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    [HideInInspector] public bool CanLoad = false;
   void Update()
    {
        if (CanLoad)
        {
            if (Input.anyKey)
            {
                SceneManager.LoadScene("Scenes/Player");
            }            
        }
    }
}
