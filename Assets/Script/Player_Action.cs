using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class Player_Action : MonoBehaviour
{
    Transform tr;
    public GameObject target;
    float shortDis;
    public List<GameObject> targets = new List<GameObject>();
    Unit_Status unit_stat;
    public GameObject llight;
    public GameObject manager;
    GameManage expManager;
    public Transform des_Tr;
    public GameObject droneProbe;
    public GameObject[] droneAttackers;
    SphereCollider sight;
    Player_Level res;
    Animator playerAnimator;
    LineRenderer bulletTrace;
    public AudioSource[] gunSounds;
    int gunSoundPlayer = 0;

    public int ammoUse;
    public bool attackable = true;

    string bossString = "EnemyBoss";
    string enemyString = "Enemy";

    Vector3 direction = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Awake()
    {
        sight = GetComponent<SphereCollider>();
        playerAnimator = GetComponent<Animator>();
        expManager = manager.GetComponent<GameManage>();
    }
    void Start()
    {
        tr = GetComponent<Transform>();
        unit_stat = GetComponentInParent<Unit_Status>();
        StartCoroutine(Attack());
        bulletTrace = GetComponent<LineRenderer>();
        res = GetComponent<Player_Level>();
        StartCoroutine(ResetTarget());
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            direction = target.transform.position - tr.position;
            tr.rotation = Quaternion.Slerp(tr.rotation, Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z)), Time.deltaTime * unit_stat.rotSpeed);
        }

        if(unit_stat.player_state == Unit_Status.State.Move)
        {
            direction = des_Tr.transform.position - tr.position;
            tr.rotation = Quaternion.Slerp(tr.rotation, Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z)), Time.deltaTime * unit_stat.rotSpeed);
        }



        //tr.rotation = Quaternion.Slerp(tr.rotation, Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z)), Time.deltaTime * unit_stat.rotSpeed);
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag(enemyString) || col.CompareTag(bossString))
        {
            targets.Add(col.gameObject);
            if(target == null)
            {
                target = col.gameObject;
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag(enemyString) || col.CompareTag(bossString))
        {
            targets.Remove(col.gameObject);
            if(targets.Count >= 1 || target == col.gameObject)
            {
                SetTarget();
            }
        }
    }

    void SetTarget()
    {
        if (targets.Count != 0)
        {
            // 첫번째를 기준으로 잡아주기
            shortDis = Vector3.Distance(gameObject.transform.position, targets[0].transform.position);

            target = targets[0]; // 첫번째를 먼저 

            foreach (GameObject found in targets)
            {
                float Distance = Vector3.Distance(tr.position, found.transform.position);

                if (Distance < shortDis) // 위에서 잡은 기준으로 거리 재기
                {
                    target = found;
                    shortDis = Distance;
                }

                if (found.CompareTag(bossString))
                {
                    target = found;
                    break;
                }
            }
        }
        else
        {
            target = null;
        }
    }

    void TargetDown(GameObject _target)
    {
        droneAttackers[0].SendMessage("TargetDown", _target, SendMessageOptions.DontRequireReceiver);
        droneAttackers[1].SendMessage("TargetDown", _target, SendMessageOptions.DontRequireReceiver);
        manager.SendMessage("Player_TargetKilled", SendMessageOptions.DontRequireReceiver);
        this.SendMessage("AmmoUsed", ammoUse, SendMessageOptions.DontRequireReceiver); //죽였을 때 탄 소모
        targets.Remove(_target);
        SetTarget();

    }

    IEnumerator Attack()
    {
        while (true)
        {
            if (target != null && attackable && unit_stat.player_state != Unit_Status.State.Dead)
            {

                if (res.res_Ammo < ammoUse)
                {
                    this.SendMessage("Weapon_Upgrade", "Pistol", SendMessageOptions.DontRequireReceiver);
                    //attackable = false;
                }
                gunSoundPlayer = 1 - gunSoundPlayer;
                gunSounds[gunSoundPlayer].pitch = Random.Range(0.9f, 1.2f);
                gunSounds[gunSoundPlayer].PlayOneShot(gunSounds[gunSoundPlayer].clip);
                
                playerAnimator.SetTrigger("Shoot");
                AtkLight();
                target.SendMessage("SetAttacker", this.gameObject, SendMessageOptions.DontRequireReceiver);
                target.SendMessage("Damaged", unit_stat.atk * unit_stat.atk_revision, SendMessageOptions.DontRequireReceiver);
                //this.SendMessage("AmmoUsed", ammoUse, SendMessageOptions.DontRequireReceiver); //공격했을 때 탄 소모
                yield return new WaitForSeconds(unit_stat.atkSpd);
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    void AtkLight()
    {
        llight.SetActive(true);
        bulletTrace.SetPosition(0, llight.transform.position);
        bulletTrace.SetPosition(1, target.transform.position + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.1f, 0.1f), Random.Range(-0.3f, 0.3f)));
        bulletTrace.enabled = true;
        Invoke("LightOff", 0.05f);
    }

    void LightOff()
    {
        llight.SetActive(false);
        bulletTrace.enabled = false;
    }

    void Farming(bool _isFarming)
    {
        attackable = !_isFarming;
        droneProbe.SendMessage("ProbeGather", !_isFarming, SendMessageOptions.DontRequireReceiver);
    }

    void SetRange(float _range)
    {
        sight.radius = _range;
    }

    void SetAmmoUse(int _ammoUse)
    {
        ammoUse = _ammoUse;
    }

    IEnumerator ResetTarget()
    {
        while (true)
        {
            if (target != null && target.GetComponent<Unit_Status>().player_state == Unit_Status.State.Dead)
            {
                targets.Remove(target);
                SetTarget();
            }

            yield return new WaitForSeconds(1f);
        }
    }
}
