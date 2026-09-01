using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class UI_NotificationSystem : MonoBehaviour
{
    public string[] texts;
    public GameObject textObject;
    public GameObject systemUI;
    SoundPlayer_UI soundPlayerUI;

    void Start()
    {
        soundPlayerUI = GameObject.FindGameObjectWithTag(SoundPlayerData.soundPlayerTag).GetComponent<SoundPlayer_UI>();
    }

    public void TextOutPut(int _type)
    {
        GameObject _text = Instantiate(textObject, systemUI.transform);
        _text.SendMessage("TextSet", texts[_type], SendMessageOptions.DontRequireReceiver);
        soundPlayerUI.UIAudioPlay(SoundPlayerData.textNotification);
    }
}
