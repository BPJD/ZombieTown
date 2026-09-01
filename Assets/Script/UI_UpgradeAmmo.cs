using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UpgradeAmmo : MonoBehaviour
{
    UI_Upgrade_Active uiActive;
    public GameObject player_act;
    Player_Level res;
    Upgrade_StatusManager statusManager;
    int clickedCode;

    [SerializeField]
    Text[] costs;
    [SerializeField]
    Text[] gains;

    int[] ammoGains = { 15, 45, 90 };
    float[] reqUpgradeTimes = { 3f, 5f, 8f };
    int[] reqPart = { 10, 25, 45 };

    SoundPlayer_UI soundPlayer;

    // Start is called before the first frame update
    void Start()
    {
        res = player_act.GetComponent<Player_Level>();
        uiActive = GetComponent<UI_Upgrade_Active>();
        statusManager = GetComponent<Upgrade_StatusManager>();
        soundPlayer = GameObject.FindGameObjectWithTag(SoundPlayerData.soundPlayerTag).GetComponent<SoundPlayer_UI>();
    }

    public void CostSet()
    {
        for(int i = 0; i < ammoGains.Length; i++)
        {
            costs[i].text = "부품 " + Mathf.RoundToInt(reqPart[i] * statusManager.reqPartDecrease).ToString();
            gains[i].text = "+ " + Mathf.RoundToInt(ammoGains[i] * statusManager.ammoIncrease).ToString();
            if(Mathf.RoundToInt(reqPart[i] * statusManager.reqPartDecrease) > res.res_Part)
            {
                costs[i].color = Color.red;
            }
            else
            {
                costs[i].color = Color.green;
            }
        }
    }

    public void Ammo_Clicked(int _ammo)
    {
        if (res.res_Part >= Mathf.RoundToInt(reqPart[_ammo] * statusManager.reqPartDecrease))
        {
            clickedCode = _ammo;
            uiActive.activatedList = UI_Upgrade_Active.State.Ammo;
            uiActive.SendMessage("Upgrade_Action", reqUpgradeTimes[clickedCode], SendMessageOptions.DontRequireReceiver);
            soundPlayer.UIAudioPlay(SoundPlayerData.buttonClicked);
        }
        else
        {
            soundPlayer.UIAudioPlay(SoundPlayerData.buttonDenied);
        }
    }

    /*
    public void Ammo_Small_Clicked()
    {
        if (res.res_Part >= Mathf.RoundToInt(reqPart[0] * statusManager.reqPartDecrease))
        {
            clickedCode = 0;
            uiActive.activatedList = UI_Upgrade_Active.State.Ammo;
            uiActive.SendMessage("Upgrade_Action", reqUpgradeTimes[clickedCode], SendMessageOptions.DontRequireReceiver);
        }
        else
        {
        }
    }

    public void Ammo_Med_Clicked()
    {
        if (res.res_Part >= Mathf.RoundToInt(reqPart[1] * statusManager.reqPartDecrease))
        {
            clickedCode = 1;
            uiActive.activatedList = UI_Upgrade_Active.State.Ammo;
            uiActive.SendMessage("Upgrade_Action", reqUpgradeTimes[clickedCode], SendMessageOptions.DontRequireReceiver);
        }
        else
        {
        }
    }

    public void Ammo_Big_Clicked()
    {
        if(res.res_Part >= Mathf.RoundToInt(reqPart[2] * statusManager.reqPartDecrease))
        {
            clickedCode = 2;
            uiActive.activatedList = UI_Upgrade_Active.State.Ammo;
            uiActive.SendMessage("Upgrade_Action", reqUpgradeTimes[clickedCode], SendMessageOptions.DontRequireReceiver);
        }
        else
        {
        }
    }
    */

    public void AmmoMakeComplete()
    {
        res.res_Ammo += Mathf.RoundToInt(ammoGains[clickedCode] * statusManager.ammoIncrease);
        res.res_Part -= Mathf.RoundToInt(reqPart[clickedCode] * statusManager.reqPartDecrease);
    }


}
