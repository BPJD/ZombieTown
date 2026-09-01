using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Player_Equip : MonoBehaviour
{
    Player_Action player_Sight;
    Player_Level player_Res;
    Unit_Status player_Stat;
    Animator player_Animator;

    public RuntimeAnimatorController pistol;
    public RuntimeAnimatorController AutoRifle;
    public RuntimeAnimatorController SniperRifle;
    public GameObject[] guns;
    public Transform[] gunFirePos;
    public GameObject gunFire;
    [SerializeField]
    AudioClip[] gunSounds;

    public float[] weaponRanges = { 12f, 15f, 7f, 30f, 20f };
    public float[] weaponAtkSpds = { 0.6f, 0.8f, 0.1f, 2f, 0.12f };
    public float[] weaponAtkRevisions = { 1f, 2f, 0.4f, 15f, 0.8f };
    public int[] weaponAmmoUses = { 0, 1, 1, 2, 2 };

    float[] default_weaponRanges = { 12f, 15f, 7f, 30f, 20f };
    float[] default_weaponAtkSpds = { 0.6f, 0.8f, 0.1f, 2f, 0.12f };
    float[] default_weaponAtkRevisions = { 1f, 2f, 0.4f, 12f, 0.8f };
    int[] default_weaponAmmoUses = { 0, 1, 1, 2, 2 };
    
    int weaponCode;


    // Start is called before the first frame update
    void Start()
    {
        player_Sight = GetComponent<Player_Action>();
        player_Res = GetComponent<Player_Level>();
        player_Stat = GetComponentInParent<Unit_Status>();
        player_Animator = GetComponent<Animator>();

        for(int i = 0; i < guns.Length; i++)
        {
            default_weaponRanges[i] = weaponRanges[i];
            default_weaponAtkSpds[i] = weaponAtkSpds[i];
            default_weaponAtkRevisions[i] = weaponAtkRevisions[i];
            default_weaponAmmoUses[i] = weaponAmmoUses[i];
        }
        
        Weapon_Upgrade("Pistol");
    }

    public void WeaponStatusSet(int _weaponCode, int _weaponStat, float _ref) //무슨 무기의 무슨 스탯을 얼마만큼
    {
        switch (_weaponStat) //사거리, 공속, 보정치, 탄약소모 순
        {
            case 0:
                weaponRanges[_weaponCode] += default_weaponRanges[_weaponCode] * _ref;
                break;
            case 1:
                weaponAtkSpds[_weaponCode] += default_weaponAtkSpds[_weaponCode] * _ref;
                break;
            case 2:
                weaponAtkRevisions[_weaponCode] += default_weaponAtkRevisions[_weaponCode] * _ref;
                break;
            case 3:
                weaponAmmoUses[_weaponCode] = default_weaponAmmoUses[_weaponCode] + (int)_ref;
                break;
        }
        if(weaponCode == _weaponCode)
        {
            weaponStatusUpdate();
        }
    }

    public void Weapon_Upgrade(string _weapon)
    {
        switch (_weapon)
        {
            //무기 기본값
            //사거리 15
            //공속 0.6

            case "SMG":
                weaponCode = 2;
                player_Animator.runtimeAnimatorController = AutoRifle;
                player_Sight.gunSounds[0].clip = gunSounds[weaponCode];
                player_Sight.gunSounds[1].clip = gunSounds[weaponCode];
                WeaponObjActive(2);
                break;

            case "Revolver":
                weaponCode = 1;
                player_Animator.runtimeAnimatorController = pistol;
                WeaponObjActive(0);
                player_Sight.gunSounds[0].clip = gunSounds[weaponCode];
                player_Sight.gunSounds[1].clip = gunSounds[weaponCode];
                break;

            case "SniperRifle":
                weaponCode = 3;
                WeaponObjActive(3);
                player_Animator.runtimeAnimatorController = SniperRifle;
                player_Sight.gunSounds[0].clip = gunSounds[weaponCode];
                player_Sight.gunSounds[1].clip = gunSounds[weaponCode];
                break;

            case "AutoRifle":
                weaponCode = 4;
                WeaponObjActive(4);
                player_Animator.runtimeAnimatorController = AutoRifle;
                player_Sight.gunSounds[0].clip = gunSounds[weaponCode];
                player_Sight.gunSounds[1].clip = gunSounds[weaponCode];
                break;

            case "Pistol":
                weaponCode = 0;
                WeaponObjActive(0);
                player_Animator.runtimeAnimatorController = pistol;
                player_Sight.gunSounds[0].clip = gunSounds[weaponCode];
                player_Sight.gunSounds[1].clip = gunSounds[weaponCode];
                break;

            case "Shotgun":

                break;
        }
        weaponStatusUpdate();

    }

    void weaponStatusUpdate()
    {
        player_Sight.SendMessage("SetRange", weaponRanges[weaponCode], SendMessageOptions.DontRequireReceiver);
        player_Stat.atk_revision = weaponAtkRevisions[weaponCode];
        player_Stat.atkSpd = weaponAtkSpds[weaponCode];
        player_Sight.SendMessage("SetAmmoUse", weaponAmmoUses[weaponCode], SendMessageOptions.DontRequireReceiver);
    }

    void WeaponObjActive(int _code)
    {
        for(int i = 0; i < guns.Length; i++)
        {
            guns[i].SetActive(false);
        }
        guns[_code].SetActive(true);
        gunFire.transform.position = gunFirePos[_code].position;
    }


}
