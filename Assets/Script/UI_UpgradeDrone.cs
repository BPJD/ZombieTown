using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.UI;

public class UI_UpgradeDrone : MonoBehaviour
{
    public GameObject player_act;
    Player_Level res;
    bool[] upgraged = { false, false, false, false, false };
    public GameObject[] drones;
    public Text[] txt_costParts;
    public Text[] txt_costAmmos;
    Image buttonImage; 
    UI_Upgrade_Active uiActive;
    Upgrade_StatusManager statusManager;

    SoundPlayer_UI soundPlayer;

    float[] reqUpgradeTimes = { 10f, 25f, 35f, 15f, 15f };
    int[] reqPart = { 350, 650, 900, 400, 400 };
    int[] reqAmmo = { 30, 60, 100, 0, 0 };
    int drone_number;

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
        for(int i = 0; i < drones.Length; i++)
        {
            txt_costParts[i].text = "부품 " + Mathf.RoundToInt(reqPart[i] * statusManager.reqPartDecrease).ToString();
            txt_costAmmos[i].text = "탄약 " + Mathf.RoundToInt(reqAmmo[i] * statusManager.reqPartDecrease).ToString();

            if (Mathf.RoundToInt(reqPart[i] * statusManager.reqPartDecrease) > res.res_Part)
            {
                txt_costParts[i].color = Color.red;
            }
            else
            {
                txt_costParts[i].color = Color.green;
            }

            if (Mathf.RoundToInt(reqAmmo[i] * statusManager.reqPartDecrease) > res.res_Ammo)
            {
                txt_costAmmos[i].color = Color.red;
            }
            else
            {
                txt_costAmmos[i].color = Color.green;
            }
        }
    }
    public void Drone_Clicked(int _drone)
    {
        SetDroneNumber(_drone);
        if (upgraged[drone_number] == true)
        {
            soundPlayer.UIAudioPlay(SoundPlayerData.buttonDenied);
        }
        else if (res.res_Part >= reqPart[drone_number] * statusManager.reqPartDecrease && res.res_Ammo >= reqAmmo[drone_number] * statusManager.reqPartDecrease)
        {
            uiActive.SendMessage("Upgrade_Action", reqUpgradeTimes[drone_number], SendMessageOptions.DontRequireReceiver);
            soundPlayer.UIAudioPlay(SoundPlayerData.buttonClicked);
        }
        else
        {
            soundPlayer.UIAudioPlay(SoundPlayerData.buttonDenied);
        }
    }

    void SetDroneNumber(int _number)
    {
        drone_number = _number;
        uiActive.activatedList = UI_Upgrade_Active.State.Drone;
    }

    void MakeDrone()
    {
        res.res_Part -= Mathf.RoundToInt(reqPart[drone_number] * statusManager.reqPartDecrease);
        res.res_Ammo -= Mathf.RoundToInt(reqAmmo[drone_number] * statusManager.reqPartDecrease);
        upgraged[drone_number] = true;
        txt_costParts[drone_number].enabled = false;
        txt_costAmmos[drone_number].enabled = false;
        drones[drone_number].SetActive(true);
        buttonImage = txt_costAmmos[drone_number].GetComponentInParent<Image>();
        buttonImage.sprite = Resources.Load<Sprite>("UI_Btn/btn_selected") as Sprite;
    }



}
