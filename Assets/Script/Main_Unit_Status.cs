using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main_Unit_Status : MonoBehaviour // 유닛의 스탯
{

    public enum State { Idle, Move, InBuilding, Suppress, Dead };
    public State player_state = State.Idle;
    public int maxHp;
    public int hp;
    public int atk;
    public float atk_revision = 1f;
    public float atkSpd;
    public float rotSpeed;
    GameObject attacker;
    public GameObject particle;
    public float siCheTime = 3f;


    void Start()
    {
        maxHp = hp;
    }

    void SetAttacker(GameObject _attacker)
    {
        attacker = _attacker;
    }

    void Damaged(float dmg)
    {
        hp -= (int)dmg;
        Instantiate(particle, this.transform);

        if(hp <= 0)
        {
            this.gameObject.SendMessage("Down",SendMessageOptions.DontRequireReceiver);
            tag = "Dead";
            player_state = State.Dead;
            attacker.SendMessage("TargetDown", this.gameObject, SendMessageOptions.DontRequireReceiver);
            Destroy(gameObject, siCheTime);
        }
    }

    void Healed(int dmg)
    {
        if(maxHp <= hp + dmg)
        {
            hp = maxHp;
        }
        else
        {
            hp += dmg;
        }
    }

}
