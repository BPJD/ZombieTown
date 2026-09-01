using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Level : MonoBehaviour
{

    int exp = 0;
    int reqExp = 100;
    float exp_per;
    public float exp_revision = 1f;
    float reqExp_revision = 0.08f;

    public int level = 1;
    public int levelAtk;
    float atk_revision = 1f;
    int default_atk;
    public int levelHp;
    //public int resMaxUp;
    public int res_Food = 30;
    public int res_Part = 0;
    public int res_Ammo = 0;
    public int res_FoodUp = 3;
    public int res_PartUp = 50;
    public int res_AmmoUp = 15;
    int res_FoodMax = 30;
    [SerializeField]
    int res_PartMax = 300;
    int res_AmmoMax = 50;

    int default_FoodMax;
    int default_PartMax;
    int default_AmmoMax;

    int enhance_foodMax;
    int enhance_partMax;
    int enhance_ammoMax;

    public float foodMax_revision = 1f;
    public float partMax_revision = 1f;
    public float ammoMax_revision = 1f;
    public float partUp_revision = 1f;
    public float foodUp_revision = 1f;
    public float ammoUp_revision = 1f;

    int foodCost = 1;

    public int rand_foodRate = 40;
    int rand_min = 12;
    int rand_max = 20;
    public float hp_revision = 1f;

    int statCount = 1;
    int levelUpCount = 0;
    public int remainCardPoint = 0;

    public float food_down_per_second = 6f;
    public float hpHealPer = 0.2f;

    int default_reqExp;
    int default_maxHp;

    Unit_Status stat;

    public Image hpBar;
    public Text txt_Hp;
    public Text txt_level;
    public Text txt_Exp;
    public Text txt_Food;
    public Text txt_Part;
    public Text txt_Ammo;
    public Text[] res_texts;
    public Image expBar;
    [SerializeField]
    Color[] resTextColors;

    public GameObject ui_ResUp;
    //public Transform farmingBtn;
    public GameObject uiCanvas;
    GameObject uiRes;

    GameManage expManager;
    UI_NotificationSystem notificationSystem;

    void Awake()
    {
        stat = GetComponentInParent<Unit_Status>();
        expManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManage>();
        notificationSystem = GameObject.FindGameObjectWithTag("GameController").GetComponentInChildren<UI_NotificationSystem>();
        default_reqExp = reqExp;

    }
    void Start()
    {
        level = 1;
        reqExp = default_reqExp;
        default_atk = stat.atk;
        default_maxHp = stat.maxHp;

        default_FoodMax = res_FoodMax;
        default_PartMax = res_PartMax;
        default_AmmoMax = res_AmmoMax;

        StatSet();

        StartCoroutine(FoodDown());
    }

    void ExpUp(int _expUp)
    {
        if(level != 50) //최대 레벨이 아닐 경우
        {
            exp += Mathf.RoundToInt(_expUp * exp_revision);

            if (exp >= reqExp)
            {
                remainCardPoint++;
                exp -= reqExp;
                LevelUp();
                reqExp += (int)(reqExp * reqExp_revision);
            }

            exp_per = (float)exp / (float)reqExp * 100; //경험치 % 로 계산
            expBar.fillAmount = exp_per * 0.01f; //경험치 바는 0~1로 표현
            txt_Exp.text = exp.ToString() + " / " + reqExp.ToString() + " (" + exp_per.ToString("F2") + " %)";
        }
        else //최대 레벨일 때
        {
            expBar.fillAmount = 0f; //경험치 바는 0~1로 표현
            exp_per = 100;
            txt_Exp.text = "Max Level";
        }
    }
    void LevelUp()
    {
        level++;
        statCount++;
        levelUpCount++;
        StatSet();
        expManager.GetComponentInChildren<Card_CallOut>().CardDraw();
    }

    void StatSet()
    {
        ExpUp(0);
        if(level != 1)
        {
            if(levelUpCount >= 5) //경험치 보정
            {
                if(level >= 35)
                {
                    reqExp_revision = 0.1f;
                }
                else
                {
                    reqExp_revision += 0.02f;
                }
                levelUpCount = 0;
            }
            if (statCount == 5) //스탯 증가
            {
                levelAtk += 5;
                AtkRevisionUp(0f);
                statCount = 0;
            }
            else
            {
                HpRevisionUp(0f);
                levelHp += 5;
            }
        }

        stat.hp += (int)(stat.maxHp * 0.3f);
        if(stat.hp >= stat.maxHp)
        {
            stat.hp = stat.maxHp;
        }
    }

    public void HpRevisionUp(float _revision)
    {
        hp_revision += _revision;
        stat.maxHp = Mathf.RoundToInt((default_maxHp + levelHp) * hp_revision);
        //Debug.Log(Mathf.RoundToInt((default_maxHp + levelHp) * hp_revision));

    }
    public void AtkRevisionUp(float _revision)
    {
        atk_revision += _revision;
        stat.atk = Mathf.RoundToInt((default_atk + levelAtk) * atk_revision);
    }

    public void MaxResourceSetup()
    {
        res_FoodMax = Mathf.RoundToInt((default_FoodMax + enhance_foodMax) * foodMax_revision);
        res_PartMax = Mathf.RoundToInt((default_PartMax + enhance_partMax) * partMax_revision);
        res_AmmoMax = Mathf.RoundToInt((default_AmmoMax + enhance_ammoMax) * ammoMax_revision);
    }

    void OnGUI()
    {
        txt_level.text = "Level : " + level.ToString();

        if (res_Food >= res_FoodMax)
        {
            res_Food = res_FoodMax;
        }
        if (res_Part >= res_PartMax)
        {
            res_Part = res_PartMax;
        }
        if (res_Ammo >= res_AmmoMax)
        {
            res_Ammo = res_AmmoMax;
        }

        txt_Hp.text = stat.hp + " / " + stat.maxHp;
        txt_Food.text = res_Food + " / " + res_FoodMax;
        txt_Part.text = res_Part + " / " + res_PartMax;
        txt_Ammo.text = res_Ammo + " / " + res_AmmoMax;
        hpBar.fillAmount = ((float)stat.hp / (float)stat.maxHp);
        UIColorChange();
    }

    void LevelReset()
    {
        StopCoroutine(FoodDown());
        level = 1;
        exp = 0;
        reqExp = 100;
    }


    void Farmed() //Building_UI 에서 호출됨, 탐사 드론에서 호출됨
    {
        int foodUp = 0;
        int partUp = Mathf.RoundToInt(Random.Range(rand_min, rand_max) * partUp_revision);
        int ammoUp = Mathf.RoundToInt(Random.Range(0, 3) * ammoUp_revision);

        if (Mathf.RoundToInt(Random.Range(0, 100)) <= rand_foodRate)
        {
            foodUp = Mathf.RoundToInt(Random.Range(2, 5) * foodUp_revision);
        }
        //int foodUp = Mathf.RoundToInt((Random.Range(rand_min, rand_max) * rand_revision) * foodUp_revision);
        
        res_Food += foodUp;
        res_Part += partUp;
        res_Ammo += ammoUp;

        int[] upRes = { foodUp, partUp, ammoUp };
        uiRes = Instantiate(ui_ResUp, Camera.main.WorldToScreenPoint(this.transform.position + new Vector3(0, 5, 0)), Quaternion.identity, uiCanvas.transform);
        //Instantiate(ui_ResUp, Camera.main.WorldToScreenPoint(new Vector3(0, 0, 0)), Quaternion.identity, uiCanvas.transform);
        //uiRes = GameObject.FindGameObjectWithTag("UI_Res");
        uiRes.SendMessage("SetRes", upRes, SendMessageOptions.DontRequireReceiver);

        ExpUp(expManager.farmingExp);

    }

    IEnumerator FoodDown()
    {
        while (true)
        {
            yield return new WaitForSeconds(food_down_per_second);
            
            if(res_Food <= 0)
            {
                stat.SendMessage("Damaged", 8, SendMessageOptions.DontRequireReceiver);
                notificationSystem.TextOutPut(2);
            }
            else
            {
                res_Food -= foodCost;

                if (stat.maxHp <= stat.hp + (int)(stat.maxHp * hpHealPer))
                {
                    stat.hp = stat.maxHp;
                }
                else
                {
                    stat.hp += (int)(stat.maxHp * hpHealPer);
                }

            }

            //UIColorChange();
        }
    }

    
    void UIColorChange()
    {
        float[] j = { 1, 1, 1 };
        float foodPer, partPer, ammoPer;

        foodPer = (float)res_Food / (float)res_FoodMax;
        partPer = (float)res_Part / (float)res_PartMax;
        ammoPer = (float)res_Ammo / (float)res_AmmoMax;

        j[0] = foodPer;
        j[1] = partPer;
        j[2] = ammoPer;

        for (int i = 0; i < 3; i++)
        {
            if (j[i] >= 0.75)
            {
                res_texts[i].color = resTextColors[3];
            }
            else if (j[i] >= 0.3)
            {
                res_texts[i].color = resTextColors[2];
            }
            else if (j[i] >= 0.1)
            {
                res_texts[i].color = resTextColors[1];
            }
            else
            {
                res_texts[i].color = resTextColors[0];
            }
        }
    }
    


    void PartUsed(int _down)
    {
        res_Part -= _down;
    }

    void AmmoUsed(int _use)
    {
        res_Ammo -= _use;
    }

    void ResourceMaxUp(string _res)
    {
        switch (_res)
        {
            case "Food":
                enhance_foodMax += res_FoodUp;
                break;
            case "Part":
                enhance_partMax += res_PartUp;
                break;
            case "Ammo":
                enhance_ammoMax += res_AmmoUp;
                break;
        }
        MaxResourceSetup();
    }

    void ResUsed(int _down)
    {
        res_Ammo -= _down;
        res_Part -= _down;
        res_Food -= _down;
    }


}
