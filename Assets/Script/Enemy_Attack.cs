using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Attack : MonoBehaviour
{
    GameObject target;
    Unit_Status unit_stat;
    Animator animator;
    Collider coll;
    float atkSpd;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        unit_stat = GetComponentInParent<Unit_Status>();
        atkSpd = unit_stat.atkSpd;
        coll = GetComponent<Collider>();
        animator = GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer < 0f && target != null)
        {
            target.SendMessage("SetAttacker", this.gameObject, SendMessageOptions.DontRequireReceiver);
            target.SendMessage("Damaged", unit_stat.atk, SendMessageOptions.DontRequireReceiver);
            animator.SetInteger("RandomMotion", (int)Random.Range(0, 2));
            animator.SetTrigger("Attack");
            timer = atkSpd;
        }

    }
    
    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player") /*|| col.CompareTag("Player")*/)
        {
            if(col.gameObject.GetComponent<Unit_Status>().player_state == Unit_Status.State.InBuilding)
            {
                target = GameObject.FindGameObjectWithTag("PlayerBuilding");
            }
            else
            {
                target = col.gameObject;
            }
        }
        else if (col.CompareTag("PlayerBuilding"))
        {
            target = col.gameObject;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player") || col.CompareTag("Building") || col.CompareTag("PlayerBuilding"))
        {
            target = null;
        }
    }

    void TargetDown(GameObject _target)
    {
        target.SendMessage("Down", SendMessageOptions.DontRequireReceiver);
        target = null;
    }
}
