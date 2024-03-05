using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private AudioSource FXaudioSource;
    [SerializeField] private AudioSource BGMaudioSource;

    [SerializeField] private Button _Openbutton;
    [SerializeField] private Button _Closebutton;
    [SerializeField] private Slider BGMslider;
    [SerializeField] private Slider FXslider;
    [SerializeField] private GameObject setUI;
    
    void Start()
    {
        DontDestroyOnLoad(this);
        FXaudioSource.PlayOneShot(clips[2]);
        BGMslider.onValueChanged.AddListener(BGMVolumeChange);
        FXslider.onValueChanged.AddListener(FXVolumeChange);
    }

    public void OnOpenClick()
    {
        setUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void OnCloseClick()
    {
        setUI.SetActive(false);
        Time.timeScale = 1f;
    }
    
    

    private void FXVolumeChange(float value)
    {
        FXaudioSource.volume = value;
    }

    private void BGMVolumeChange(float value)
    {
        BGMaudioSource.volume = value;
    }

    public void PlayBGM()
    {
        BGMaudioSource.PlayOneShot(clips[1]);
    }

    public void PlayJump()
    {
        FXaudioSource.PlayOneShot(clips[5]);
    }

    public void PlayCameraRotate(int num)
    {
        switch (num)
        {
            case -1: //Left Rotate
                FXaudioSource.PlayOneShot(clips[6]);
                break;
            
            case 1: //Right Rotate
                FXaudioSource.PlayOneShot(clips[7]);
                break;
        }
    }

    public void PlayDoorFX(int num)
    {
        switch (num)
        {
            case 0: //Enter
                FXaudioSource.PlayOneShot(clips[3]);
                break;
            
            case 1: //Exit
                FXaudioSource.PlayOneShot(clips[4]);
                break;
        }
        
    }

    public void PlayFallFX()
    {
        FXaudioSource.PlayOneShot(clips[0]);
    }
}

