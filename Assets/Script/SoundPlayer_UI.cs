using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer_UI : MonoBehaviour
{
    AudioSource audioSource;

    public AudioClip[] clips;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void UIAudioPlay(int _code)
    {
        audioSource.PlayOneShot(clips[_code]);
    }
}



public static class SoundPlayerData
{
    public static string soundPlayerTag = "SoundPlayer";

    public static int buttonClicked = 0;
    public static int buildingIn = 1;
    public static int buildingOut = 2;
    public static int upgrading = 3;
    public static int upgradeComplete = 4;
    public static int upgradecancel = 5;
    public static int cameraPlus = 6;
    public static int cameraMinus = 7;
    public static int upgradeClose = 8;
    public static int upgradeOpen = 9;
    public static int repaired = 10;
    public static int textNotification = 11;
    public static int farmingComplete = 12;
    public static int farmingStart = 13;
    public static int buttonDenied = 14;
}
