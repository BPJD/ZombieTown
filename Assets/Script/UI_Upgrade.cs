using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Upgrade : MonoBehaviour
{
    public GameObject[] listes; //0 무기, 1 강화, 2 드론, 3 타워, 4 탄약
    public Image[] button;
    public enum State { Weapon, Enhance, Drone, Tower, Ammo };
    public State activatedList = State.Weapon;
    SoundPlayer_UI soundPlayer;

    void Start()
    {
        soundPlayer = GameObject.FindGameObjectWithTag(SoundPlayerData.soundPlayerTag).GetComponent<SoundPlayer_UI>();
    }
    public void ListClicked(int _code)
    {
        ButtonClicked(_code);
        soundPlayer.UIAudioPlay(SoundPlayerData.buttonClicked);
    }
    /*
    public void WeaponListClicked()
    {
        ButtonClicked(0);
    }

    public void EnhanceListClicked()
    {
        ButtonClicked(1);
    }

    public void DroneListClicked()
    {
        ButtonClicked(2);
    }

    public void TowerListClicked()
    {
        ButtonClicked(3);
    }

    public void AmmoListClicked()
    {
        ButtonClicked(4);
    }
    */

    void ButtonClicked(int _clicked)
    {
        ActivateList((int)activatedList, false);
        activatedList = (State)_clicked;
        ActivateList((int)activatedList, true);
        soundPlayer.UIAudioPlay(SoundPlayerData.buttonClicked);
    }

    void ActivateList(int _list, bool _isActivate)
    {
        listes[_list].SetActive(_isActivate);

        if (_isActivate)
        {
            button[_list].sprite = Resources.Load<Sprite>("UI_Btn/btn_selected") as Sprite;
        }
        else
        {
            button[_list].sprite = Resources.Load<Sprite>("UI_Btn/btn_idle") as Sprite;
        }
    }
}
