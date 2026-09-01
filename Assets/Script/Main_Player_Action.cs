using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class Main_Player_Action : MonoBehaviour
{
    Transform tr;
    public GameObject target;
    float shortDis;
    public List<GameObject> targets = new List<GameObject>();
    Main_Unit_Status unit_stat;
    public GameObject llight;
    public Transform des_Tr;
    public GameObject droneAttacker;
    SphereCollider sight;
    Animator playerAnimator;

    public bool attackable = true;

    Vector3 direction = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Awake()
    {
        sight = GetComponent<SphereCollider>();
        playerAnimator = GetComponent<Animator>();
    }
    void Start()
    {
        tr = GetComponent<Transform>();
        unit_stat = GetComponentInParent<Main_Unit_Status>();
        StartCoroutine(Attack());
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            direction = target.transform.position - tr.position;
            tr.rotation = Quaternion.Slerp(tr.rotation, Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z)), Time.deltaTime * unit_stat.rotSpeed);
        }

        if(unit_stat.player_state == Main_Unit_Status.State.Move)
        {
            direction = des_Tr.transform.position - tr.position;
            tr.rotation = Quaternion.Slerp(tr.rotation, Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z)), Time.deltaTime * unit_stat.rotSpeed);
        }



        //tr.rotation = Quaternion.Slerp(tr.rotation, Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z)), Time.deltaTime * unit_stat.rotSpeed);
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Enemy")
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
        if(col.tag == "Enemy")
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
        droneAttacker.SendMessage("TargetDown", _target, SendMessageOptions.DontRequireReceiver);
        targets.Remove(_target);
        SetTarget();
    }

    IEnumerator Attack()
    {
        while (true)
        {
            if (target != null && attackable)
            {
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
        Invoke("LightOff", 0.05f);
    }

    void LightOff()
    {
        llight.SetActive(false);
    }


    void SetRange(float _range)
    {
        sight.radius = _range;
    }

}
