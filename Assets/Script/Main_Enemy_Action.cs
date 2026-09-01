using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Main_Enemy_Action : MonoBehaviour
{

    Rigidbody rig;
    Transform player;
    Transform tr;
    Main_Unit_Status stat;
    NavMeshAgent navMesh;
    Collider col;
    public GameObject target;
    public GameObject zombieArm;
    public BoxCollider[] boxCols;
    bool isAttacking = false;

    Animator animator;


    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        rig = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        stat = GetComponent<Main_Unit_Status>();
        navMesh = GetComponent<NavMeshAgent>();
        col = GetComponent<Collider>();
        animator = GetComponent<Animator>();
        player = target.transform;
    }

    void Start()
    {
        animator.SetFloat("MoveSpeed", navMesh.speed);
        ChasePlayer();
    }

    void ChasePlayer()
    {
        if (target != null && !isAttacking)
        {
            animator.SetBool("HasTarget", true);
            navMesh.SetDestination(player.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if(target != null)
        //{
        //    Vector3 direction = player.position - tr.position;
        //    tr.rotation = Quaternion.Slerp(tr.rotation, Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z)), Time.deltaTime * stat.rotSpeed);
        //}
    }

    void TargetDown(GameObject _target)
    {
        navMesh.isStopped = true;
    }

    void Down()
    {
        navMesh.enabled = false;
        target = null;
        rig.useGravity = true;
        col.enabled = false;
        Destroy(zombieArm);
        boxCols[0].enabled = true;
        boxCols[1].enabled = true;
        boxCols[2].enabled = true;

        animator.SetInteger("RandomMotion", Random.Range(0, 3));
        animator.SetBool("IsDied", true);
        animator.SetTrigger("Die");
    }

    void MoveSpeedDown()
    {
        navMesh.speed = 2f;
        animator.SetFloat("MoveSpeed", navMesh.speed);
    }



    
}
