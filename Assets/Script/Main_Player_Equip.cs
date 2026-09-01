using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Player_Equip : MonoBehaviour
{
    Main_Player_Action player_Sight;
    Main_Unit_Status player_Stat;
    Animator player_Animator;

    public RuntimeAnimatorController pistol;
    public RuntimeAnimatorController AutoRifle;
    public RuntimeAnimatorController SniperRifle;
    public GameObject[] guns;
    public Transform[] gunFirePos;
    public GameObject gunFire;



    // Start is called before the first frame update
    void Start()
    {
        player_Sight = GetComponent<Main_Player_Action>();
        player_Stat = GetComponentInParent<Main_Unit_Status>();
        player_Animator = GetComponent<Animator>();
        
        Weapon_Upgrade("AutoRifle");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Weapon_Upgrade(string _weapon)
    {
        switch (_weapon)
        {
            //무기 기본값
            //사거리 15
            //공속 0.6

            case "SMG":
                player_Animator.runtimeAnimatorController = AutoRifle;
                WeaponObjActive(2);
                player_Sight.SendMessage("SetRange", 10f, SendMessageOptions.DontRequireReceiver);
                player_Stat.atk_revision = 0.3f;
                player_Stat.atkSpd = 0.1f;
                player_Sight.SendMessage("SetAmmoUse", 1, SendMessageOptions.DontRequireReceiver);
                break;
            case "Revolver":
                player_Animator.runtimeAnimatorController = pistol;
                WeaponObjActive(0);
                player_Sight.SendMessage("SetRange", 15f, SendMessageOptions.DontRequireReceiver);
                player_Stat.atk_revision = 1.5f;
                player_Stat.atkSpd = 0.8f;
                player_Sight.SendMessage("SetAmmoUse", 1, SendMessageOptions.DontRequireReceiver);
                break;
            case "SniperRifle":
                WeaponObjActive(3);
                player_Animator.runtimeAnimatorController = SniperRifle;
                player_Sight.SendMessage("SetRange", 40f, SendMessageOptions.DontRequireReceiver);
                player_Stat.atk_revision = 5f;
                player_Stat.atkSpd = 3.5f;
                player_Sight.SendMessage("SetAmmoUse", 2, SendMessageOptions.DontRequireReceiver);
                break;
            case "AutoRifle":
                WeaponObjActive(4);
                player_Animator.runtimeAnimatorController = AutoRifle;
                player_Sight.SendMessage("SetRange", 12f, SendMessageOptions.DontRequireReceiver);
                player_Stat.atk_revision = 1f;
                player_Stat.atkSpd = 0.15f;
                break;
            case "Pistol":
                WeaponObjActive(0);
                player_Animator.runtimeAnimatorController = pistol;
                player_Sight.SendMessage("SetRange", 15f, SendMessageOptions.DontRequireReceiver);
                player_Stat.atk_revision = 1f;
                player_Stat.atkSpd = 0.6f;
                player_Sight.SendMessage("SetAmmoUse", 0, SendMessageOptions.DontRequireReceiver);
                break;
            case "Shotgun":

                break;
        }
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
