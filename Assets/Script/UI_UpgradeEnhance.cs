using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.UI;

public class UI_UpgradeEnhance : MonoBehaviour
{
    UI_Upgrade_Active uiActive;
    public GameObject player_act;
    public Text txt_action_remainTime;
    Player_Level res;
    Upgrade_StatusManager statusManager;
    public string resource;

    [SerializeField]
    Text[] costs;
    [SerializeField]
    Text[] explains;

    float reqUpgradeTime = 5f;
    public int reqPart = 25;

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
        for(int i = 0; i < costs.Length; i++)
        {
            costs[i].text = "부품 " + Mathf.RoundToInt(reqPart * statusManager.reqPartDecrease).ToString();
            if(Mathf.RoundToInt(reqPart * statusManager.reqPartDecrease) > res.res_Part)
            {
                costs[i].color = Color.red;
            }
            else
            {
                costs[i].color = Color.green;
            }

        }
        explains[0].text = "+" + Mathf.RoundToInt(res.res_FoodUp * res.foodMax_revision);
        explains[1].text = "+" + Mathf.RoundToInt(res.res_PartUp * res.partMax_revision);
        explains[2].text = "+" + Mathf.RoundToInt(res.res_AmmoUp * res.ammoMax_revision);
    }

    public void Enhance_Food_Clicked()
    {
        if (res.res_Part >= Mathf.RoundToInt(reqPart * statusManager.reqPartDecrease))
        {
            uiActive.activatedList = UI_Upgrade_Active.State.Enhance;
            resource = "Food";
            uiActive.SendMessage("Upgrade_Action", reqUpgradeTime, SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            soundPlayer.UIAudioPlay(SoundPlayerData.buttonDenied);
        }
    }

    public void Enhance_Part_Clicked()
    {
        if (res.res_Part >= Mathf.RoundToInt(reqPart * statusManager.reqPartDecrease))
        {
            uiActive.activatedList = UI_Upgrade_Active.State.Enhance;
            resource = "Part";
            uiActive.SendMessage("Upgrade_Action", reqUpgradeTime, SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            soundPlayer.UIAudioPlay(SoundPlayerData.buttonDenied);
        }
    }

    public void Enhance_Ammo_Clicked()
    {
        if(res.res_Part >= Mathf.RoundToInt(reqPart * statusManager.reqPartDecrease))
        {
            uiActive.activatedList = UI_Upgrade_Active.State.Enhance;
            resource = "Ammo";
            uiActive.SendMessage("Upgrade_Action", reqUpgradeTime, SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            soundPlayer.UIAudioPlay(SoundPlayerData.buttonDenied);
        }
    }

    

    /*
    public void Enhance_resFarm_Clicked()
    {
        if (res.res_Part >= reqRes && res.res_Food >= reqRes && res.res_Ammo >= reqRes)
        {
            uiActive.activatedList = UI_Upgrade_Active.State.Resource;
            uiActive.SendMessage("Upgrade_Action", reqUpgradeTime, SendMessageOptions.DontRequireReceiver);
        }
        else
        {
        }
    }
    */


}
