using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class UI_Upgrade_Active : MonoBehaviour
{
    SoundPlayer_UI uiSoundPlayer;
    public GameObject player_act;
    public GameObject action_UI;
    public GameObject upgradeListUI;
    public GameObject buildingUI;
    public GameObject dPadManager;
    UI_UpgradeEquip equip;
    UI_UpgradeEnhance enhance;
    UI_UpgradeDrone drone;
    UI_UpgradeAmmo ammo;
    Upgrade_StatusManager statusManager;
    public Text txt_action_remainTime;
    float upgradeTimer;
    public bool isUpgradeAction = false;

    public enum State { Weapon, Enhance, Drone, Tower, Ammo };
    public State activatedList = State.Weapon;


    // Start is called before the first frame update
    void Start()
    {
        uiSoundPlayer = GameObject.FindGameObjectWithTag(SoundPlayerData.soundPlayerTag).GetComponent<SoundPlayer_UI>();
        equip = GetComponent<UI_UpgradeEquip>();
        enhance = GetComponent<UI_UpgradeEnhance>();
        drone = GetComponent<UI_UpgradeDrone>();
        ammo = GetComponent<UI_UpgradeAmmo>();
        statusManager = GetComponent<Upgrade_StatusManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isUpgradeAction)
        {
            upgradeTimer -= Time.deltaTime;
            txt_action_remainTime.text = "남은 시간 : " + upgradeTimer.ToString("F1");
            if (upgradeTimer <= 0f)
            {
                upgradeListUI.SetActive(true);
                action_UI.SetActive(false);
                player_act.SendMessage("Farming", false, SendMessageOptions.DontRequireReceiver);
                isUpgradeAction = false;
                buildingUI.SetActive(true);

                switch (activatedList)
                {
                    case State.Weapon:
                        equip.SendMessage("ChangeWeapon", SendMessageOptions.DontRequireReceiver);
                        equip.SendMessage("WeaponPartUsed", SendMessageOptions.DontRequireReceiver);
                        break;
                    case State.Enhance:
                        player_act.SendMessage("ResourceMaxUp", enhance.resource, SendMessageOptions.DontRequireReceiver);
                        player_act.SendMessage("PartUsed", Mathf.RoundToInt(enhance.reqPart * statusManager.reqPartDecrease), SendMessageOptions.DontRequireReceiver);
                        statusManager.EnhanceCounter(enhance.resource);
                        break;
                    case State.Drone:
                        equip.SendMessage("MakeDrone", SendMessageOptions.DontRequireReceiver);
                        WeaponUI_Open();
                        break;
                    case State.Tower:

                        break;
                    case State.Ammo:
                        ammo.AmmoMakeComplete();
                        break;
                }
                uiSoundPlayer.UIAudioPlay(SoundPlayerData.upgradeComplete);
            }
        }
    }

    public void WeaponUI_Open()
    {
        equip.CostSet();
        enhance.CostSet();
        drone.CostSet();
        ammo.CostSet();
        if (upgradeListUI.activeInHierarchy || isUpgradeAction)
        {
            upgradeListUI.SetActive(false);
            dPadManager.SendMessage("DPadActive", SendMessageOptions.DontRequireReceiver);
            uiSoundPlayer.UIAudioPlay(SoundPlayerData.upgradeOpen);
        }
        else
        {
            upgradeListUI.SetActive(true);
            dPadManager.SendMessage("DPadActive", SendMessageOptions.DontRequireReceiver);
            uiSoundPlayer.UIAudioPlay(SoundPlayerData.upgradeClose);
        }


    }

    void Upgrade_Action(float _time)
    {
        upgradeTimer = _time * statusManager.timeDecrease;
        isUpgradeAction = true;
        player_act.SendMessage("Farming", true, SendMessageOptions.DontRequireReceiver);
        action_UI.SetActive(true);
        buildingUI.SetActive(false);
        WeaponUI_Open();
        uiSoundPlayer.UIAudioPlay(SoundPlayerData.upgrading);
    }


    public void Cancel()
    {
        buildingUI.SetActive(true);
        upgradeListUI.SetActive(true);
        action_UI.SetActive(false);
        player_act.SendMessage("Farming", false, SendMessageOptions.DontRequireReceiver);
        isUpgradeAction = false;
        uiSoundPlayer.UIAudioPlay(SoundPlayerData.upgradecancel);
    }
}
