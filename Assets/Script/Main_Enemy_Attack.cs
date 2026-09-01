using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main_Enemy_Attack : MonoBehaviour
{
    GameObject target;
    Unit_Status unit_stat;
    Animator animator;
    Collider coll;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        unit_stat = GetComponentInParent<Unit_Status>();
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
        }

    }
    
    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player") || col.CompareTag("PlayerBuilding"))
        {
            target = col.gameObject;

        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player") || col.CompareTag("PlayerBuilding") || col.tag == "Building")
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
