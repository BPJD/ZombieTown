using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UpgradeEquip : MonoBehaviour
{
    public GameObject player_act;
    Player_Level res;
    string weapon;
    bool[] upgraged = { true, false, false, false, false };
    public Text[] upgradeAble;
    public Text[] equipExplains;
    UI_Upgrade_Active uiActive;
    public UI_ButtonColorManager colorManager;
    Upgrade_StatusManager statusManager;
    Player_Equip playerEquip;
    SoundPlayer_UI soundPlayer;

    string[] weaponStr = { "Pistol", "Revolver", "SMG", "SniperRifle", "AutoRifle" };
    float[] tier_reqUpgradeTime = { 8f, 15f };
    int[] tier_reqPart = { 400, 750 };
    int[] weaponTiers = {0, 0, 0, 1, 1 };


    // Start is called before the first frame update
    void Start()
    {
        res = player_act.GetComponent<Player_Level>();
        uiActive = GetComponent<UI_Upgrade_Active>();
        statusManager = GetComponent<Upgrade_StatusManager>();
        playerEquip = player_act.GetComponent<Player_Equip>();
        soundPlayer = GameObject.FindGameObjectWithTag(SoundPlayerData.soundPlayerTag).GetComponent<SoundPlayer_UI>();
    }

    public void CostSet()
    {
        for(int i = 0; i < upgraged.Length; i++)
        {
            equipExplains[i].text = "공격력 배율 : " + playerEquip.weaponAtkRevisions[i].ToString("F2") + "x" + '\n' + "공격 속도 : " + playerEquip.weaponAtkSpds[i].ToString("F2") + '\n' + "사거리 : " + playerEquip.weaponRanges[i].ToString("F2");
            upgradeAble[i].text = "부품 " + Mathf.RoundToInt(tier_reqPart[weaponTiers[i]] * statusManager.reqPartDecrease).ToString();
            if(Mathf.RoundToInt(tier_reqPart[weaponTiers[i]] * statusManager.reqPartDecrease) > res.res_Part)
            {
                upgradeAble[i].color = Color.red;
            }
            else
            {
                upgradeAble[i].color = Color.green;
            }
        }
    }

    public void Weapon_Pist_Clicked()
    {
        uiActive.activatedList = UI_Upgrade_Active.State.Weapon;
        weapon = "Pistol";
        ChangeWeapon();
    }

    public void Weapon_Clicked(int _weapon)
    {
        uiActive.activatedList = UI_Upgrade_Active.State.Weapon;
        if (upgraged[_weapon] == true)
        {
            weapon = weaponStr[_weapon];
            ChangeWeapon();
            soundPlayer.UIAudioPlay(SoundPlayerData.buttonClicked);
        }
        else if (res.res_Part >= Mathf.RoundToInt(tier_reqPart[weaponTiers[_weapon]] * statusManager.reqPartDecrease))
        {
            weapon = weaponStr[_weapon];
            uiActive.SendMessage("Upgrade_Action", tier_reqUpgradeTime[weaponTiers[_weapon]], SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            soundPlayer.UIAudioPlay(SoundPlayerData.buttonDenied);
        }
    }

    /*
    public void Weapon_Revolver_Clicked()
    {
        uiActive.activatedList = UI_Upgrade_Active.State.Weapon;
        if (upgraged[1] == true)
        {
            weapon = "Revolver";
            ChangeWeapon();
        }
        else if (res.res_Part >= Mathf.RoundToInt(tier_reqPart[0] * statusManager.reqPartDecrease))
        {
            weapon = "Revolver";
            uiActive.SendMessage("Upgrade_Action", tier_reqUpgradeTime[0], SendMessageOptions.DontRequireReceiver);
        }
        else
        {
        }
    }

    public void Weapon_SMG_Clicked()
    {
        uiActive.activatedList = UI_Upgrade_Active.State.Weapon;
        if (upgraged[2] == true)
        {
            weapon = "SMG";
            ChangeWeapon();
        }
        else if(res.res_Part >= Mathf.RoundToInt(tier_reqPart[0] * statusManager.reqPartDecrease))
        {
            weapon = "SMG";
            uiActive.SendMessage("Upgrade_Action", tier_reqUpgradeTime[0], SendMessageOptions.DontRequireReceiver);
        }
        else
        {
        }
    }

    public void Weapon_SR_Clicked()
    {
        uiActive.activatedList = UI_Upgrade_Active.State.Weapon;
        if (upgraged[3] == true)
        {
            weapon = "SniperRifle";
            ChangeWeapon();
        }
        else if (res.res_Part >= Mathf.RoundToInt(tier_reqPart[1] * statusManager.reqPartDecrease))
        {
            weapon = "SniperRifle";
            uiActive.SendMessage("Upgrade_Action", tier_reqUpgradeTime[1], SendMessageOptions.DontRequireReceiver);
        }
        else
        {
        }
    }
    public void Weapon_AR_Clicked()
    {
        uiActive.activatedList = UI_Upgrade_Active.State.Weapon;
        if (upgraged[4] == true)
        {
            weapon = "AutoRifle";
            ChangeWeapon();
        }
        else if (res.res_Part >= Mathf.RoundToInt(tier_reqPart[1] * statusManager.reqPartDecrease))
        {
            weapon = "AutoRifle";
            uiActive.SendMessage("Upgrade_Action", tier_reqUpgradeTime[1], SendMessageOptions.DontRequireReceiver);
        }
        else
        {
        }
    }
    */

    void ChangeWeapon()
    {
        colorManager.WeaponButtonColorSet(weapon);
        player_act.SendMessage("Weapon_Upgrade", weapon, SendMessageOptions.DontRequireReceiver);
        uiActive.SendMessage("WeaponUI_Open", SendMessageOptions.DontRequireReceiver);
    }

    void WeaponPartUsed()
    {
        switch (weapon)
        {
            case "Revolver":
                res.res_Part -= Mathf.RoundToInt(tier_reqPart[0] * statusManager.reqPartDecrease);
                upgraged[1] = true;
                upgradeAble[1].enabled = false;
                break;
            case "SMG":
                res.res_Part -= Mathf.RoundToInt(tier_reqPart[0] * statusManager.reqPartDecrease);
                upgraged[2] = true;
                upgradeAble[2].enabled = false;
                break;
            case "SniperRifle":
                res.res_Part -= Mathf.RoundToInt(tier_reqPart[1] * statusManager.reqPartDecrease);
                upgraged[3] = true;
                upgradeAble[3].enabled = false;
                break;
            case "AutoRifle":
                res.res_Part -= Mathf.RoundToInt(tier_reqPart[1] * statusManager.reqPartDecrease);
                upgraged[4] = true;
                upgradeAble[4].enabled = false;
                break;
        }
    }




}
