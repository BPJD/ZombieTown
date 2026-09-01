using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit_Status : MonoBehaviour // 유닛의 스탯
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
    public float siCheTime = 5f;

    int exp;

    int bossAtk = 20;
    int spawnCount = 0;

    public Text txt_hp;

    GameManage manager;
    LightRotate dayCounter;

    void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManage>();
        dayCounter = GameObject.FindGameObjectWithTag("Sun").GetComponent<LightRotate>();
    }

    void Start()
    {
        
        if(this.CompareTag("Enemy"))
        {
            StatUp();
            if (dayCounter.isDay)
            {
                this.SendMessage("MoveSpeedDown", SendMessageOptions.DontRequireReceiver);
            }
            if (dayCounter.isBossNight)
            {
                hp = (int)(hp * 0.25f);
            }
        }
        else if (this.CompareTag("EnemyBoss"))
        {
            BossStatSet();
            if (dayCounter.isDay)
            {
                this.SendMessage("MoveSpeedDown", SendMessageOptions.DontRequireReceiver);
            }
        }
        
        maxHp = hp;


    }

    void BossStatSet()
    {
        spawnCount++;

        switch (spawnCount)
        {
            case 1:
                exp = 600;
                break;
            case 2:
                exp = 1500;
                break;
            case 3:
                exp = 3000;
                break;
            case 4:
                exp = 3800;
                break;
            case 5:
                exp = 6600;
                break;
            case 6:
                exp = 8000;
                break;
            case 7:
                exp = 10000;
                break;
            default:
                exp = 0;
                break;
        }

        if(spawnCount == 7)
        {
            atk = bossAtk + (15 * spawnCount);
            hp = (25 + manager.enemyHpUpTotal) * 13;
        }
        else
        {
            //atk = bossAtks[spawnCount];
            atk = bossAtk + (15 * spawnCount);
            hp = (25 + manager.enemyHpUpTotal) * 13;
        }

    }

    void StatUp()
    {
        exp = manager.killingExp;
        atk += Mathf.FloorToInt(manager.enemyAtkUpTotal);
        hp += manager.enemyHpUpTotal;
        hp = Mathf.RoundToInt(hp + (1f + manager.enemyHpRevision));

    }

    void SetAttacker(GameObject _attacker)
    {
        attacker = _attacker;
    }

    void Damaged(float dmg)
    {
        hp -= (int)dmg;
        //Instantiate(particle, this.transform);
        Instantiate(particle, this.transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.3f, 0.3f), Random.Range(-0.5f, 0.5f)), Quaternion.identity, this.transform);

        if (this.gameObject.CompareTag("Player"))
        {
            if(hp <= 0)
            {
                attacker = GameObject.FindGameObjectWithTag("Enemy");
                hp = 0;
                Animator playerAnimator = GetComponentInChildren<Animator>();
                playerAnimator.SetTrigger("Dead");
                attacker.SendMessage("TargetDown", this.gameObject, SendMessageOptions.DontRequireReceiver);
                player_state = State.Dead;
                Destroy(gameObject, siCheTime);
            }
        }

        if(this.gameObject.CompareTag("Enemy") || this.gameObject.CompareTag("EnemyBoss"))
        {
            if(hp <= 0)
            {
                this.gameObject.SendMessage("Down", SendMessageOptions.DontRequireReceiver);
                tag = "Dead";
                attacker.SendMessage("ExpUp", exp, SendMessageOptions.DontRequireReceiver);
                player_state = State.Dead;
                attacker.SendMessage("TargetDown", this.gameObject, SendMessageOptions.DontRequireReceiver);
                Destroy(gameObject, siCheTime);

            }
        }
    }

    void OnDestroy()
    {
        if (this.CompareTag("Player") && player_state == State.Dead)
        {
            manager.SendMessage("Player_Dead",SendMessageOptions.DontRequireReceiver);
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
        /*
        if(this.gameObject.tag == "Player")
        {
            txt_hp.text = hp.ToString() + " / " + maxHp.ToString();
            hpBar.fillAmount = (float)hp / (float)maxHp * 100;
        }
        */
    }

    public void NanoHealerActive()
    {
        StartCoroutine(NanoHeal());
    }

    IEnumerator NanoHeal()
    {
        while(true)
        {
            yield return new WaitForSeconds(3f);
            Healed(Mathf.RoundToInt(maxHp * 0.02f));
        }
    }
}
