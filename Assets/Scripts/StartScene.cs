using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    [HideInInspector] public bool CanLoad = false;
    private AudioManager _audioManager;

    private void Start()
    {
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    void Update()
    {
        if (CanLoad)
        {
            if (Input.anyKey)
            {
                SceneManager.LoadScene("Scenes/Player");
                _audioManager.PlayBGM();
                Destroy(this);
            }            
        }
    }
}
