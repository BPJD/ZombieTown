using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_UpDownSystem : MonoBehaviour
{
    public GameObject[] cameras;
    public int selectedCamera = 2;
    SoundPlayer_UI soundPlayer;

    void Start()
    {
        soundPlayer = GameObject.FindGameObjectWithTag(SoundPlayerData.soundPlayerTag).GetComponent<SoundPlayer_UI>();
    }
    public void CameraUp()
    {
        if(selectedCamera + 1 != cameras.Length)
        {
            selectedCamera++;
            CameraSet();
            soundPlayer.UIAudioPlay(SoundPlayerData.cameraPlus);
        }
        else
        {
            soundPlayer.UIAudioPlay(SoundPlayerData.buttonDenied);
        }
    }

    public void CameraDown()
    {
        if(selectedCamera - 1 != -1)
        {
            selectedCamera--;
            CameraSet();
            soundPlayer.UIAudioPlay(SoundPlayerData.cameraMinus);
        }
        else
        {
            soundPlayer.UIAudioPlay(SoundPlayerData.buttonDenied);
        }
    }

    void CameraSet()
    {
        for(int i = 0; i < 5; i++)
        {
            if (cameras[i] == cameras[selectedCamera])
            {
                cameras[i].SetActive(true);
            }
            else if (cameras[i] != cameras[selectedCamera])
            {
                cameras[i].SetActive(false);
            }
        }
    }
}
