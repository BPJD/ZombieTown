using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Drone_Attacker : MonoBehaviour
{
    public GameObject player;
    Player_Level player_res;
    Unit_Status unit_stat;

    public Transform gunTr;
    Transform targetTr;
    Transform tr;
    GameObject target;
    float shortDis;
    List<GameObject> targets = new List<GameObject>();
    Vector3 direction = new Vector3(0, 0, 0);

    LineRenderer bulletTrace;

    public float atkSpd = 0.3f;
    public float atkRevision = 0.3f;
    public float scoutRevision = 1f;
    float rotSpd = 30f;
    

    AudioSource gunSound;


    // Start is called before the first frame update
    void Start()
    {
        bulletTrace = GetComponent<LineRenderer>();
        gunSound = GetComponent<AudioSource>();
        tr = GetComponent<Transform>();
        unit_stat = player.GetComponentInParent<Unit_Status>();
        player_res = player.GetComponent<Player_Level>();
        StartCoroutine(Attack());
        StartCoroutine(ResetTarget());
    }

    // Update is called once per frame
    void Update()
    {

        if (target != null)
        {
            direction = target.transform.position - tr.position;
            tr.rotation = Quaternion.Slerp(tr.rotation, Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z)), Time.deltaTime * rotSpd);
        }
        //tr.rotation = Quaternion.Slerp(tr.rotation, Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z)), Time.deltaTime * 30f);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Enemy"))
        {
            targets.Add(col.gameObject);
            if (target == null)
            {
                target = col.gameObject;
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Enemy"))
        {
            targets.Remove(col.gameObject);
            if (targets.Count >= 1 || target == col.gameObject)
            {
                SetTarget();
            }
        }
    }

    void SetTarget()
    {
        if (targets.Count != 0)
        {

            shortDis = Vector3.Distance(gameObject.transform.position, targets[0].transform.position); // 첫번째를 기준으로 잡아주기 

            target = targets[0]; // 첫번째를 먼저 

            foreach (GameObject found in targets)
            {
                float Distance = Vector3.Distance(tr.position, found.transform.position);

                if (Distance < shortDis) // 위에서 잡은 기준으로 거리 재기
                {
                    target = found;
                    shortDis = Distance;
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
        targets.Remove(_target);
        SetTarget();
    }

    IEnumerator Attack()
    {
        while (true)
        {
            if (target != null && player_res.res_Ammo >= 1)
            {
                targetTr = target.transform;
                target.SendMessage("SetAttacker", player, SendMessageOptions.DontRequireReceiver);
                target.SendMessage("Damaged", unit_stat.atk * atkRevision, SendMessageOptions.DontRequireReceiver);
                AtkLight();
                gunSound.pitch = Random.Range(0.9f, 1.2f);
                gunSound.PlayOneShot(gunSound.clip);
                //this.SendMessage("AmmoUsed", ammoUse, SendMessageOptions.DontRequireReceiver); //공격했을 때 탄 소모
                yield return new WaitForSeconds(atkSpd);
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    IEnumerator ResetTarget()
    {
        while (true)
        {
            if(target != null && target.GetComponent<Unit_Status>().player_state == Unit_Status.State.Dead)
            {
                targets.Remove(target);
                SetTarget();
            }

            yield return new WaitForSeconds(1f);
        }
    }

    void AtkLight()
    {
        bulletTrace.SetPosition(0, gunTr.position);
        bulletTrace.SetPosition(1, targetTr.position + new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.1f, 0.1f), Random.Range(-0.2f, 0.2f)));
        bulletTrace.enabled = true;
        Invoke("LightOff", 0.05f);
    }

    void LightOff()
    {
        bulletTrace.enabled = false;
    }
}
