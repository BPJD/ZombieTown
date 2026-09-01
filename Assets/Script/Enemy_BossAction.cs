using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_BossAction : MonoBehaviour
{
    GameManage manager;

    Rigidbody rig;
    Transform player;
    Transform tr;
    Unit_Status stat;
    NavMeshAgent navMesh;
    LightRotate dayCounter;
    Collider col;
    public GameObject target;
    public GameObject zombieArm;
    public BoxCollider[] boxCols;
    bool isAttacking = false;

    Animator animator;


    void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManage>();
        target = GameObject.FindGameObjectWithTag("Player");
        rig = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        stat = GetComponent<Unit_Status>();
        navMesh = GetComponent<NavMeshAgent>();
        col = GetComponent<Collider>();
        animator = GetComponent<Animator>();
        dayCounter = GameObject.FindGameObjectWithTag("Sun").GetComponent<LightRotate>();
        player = target.transform;
    }

    void Start()
    {
        
        animator.SetFloat("MoveSpeed", navMesh.speed);
        StartCoroutine(ChasePlayer());
    }

    IEnumerator ChasePlayer()
    {
        while (true)
        {
            if(target != null && !isAttacking)
            {
                animator.SetBool("HasTarget", true);
                navMesh.SetDestination(player.position);
            }
            else if(stat.player_state != Unit_Status.State.Dead)
            {
                animator.SetBool("HasTarget", false);
                navMesh.ResetPath();
            }
            yield return new WaitForSeconds(1f);
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
        animator.SetFloat("MoveSpeed", navMesh.speed);
    }

    void OnDestroy()
    {
        if(dayCounter.dayCount >= 21)
        {
            manager.SendMessage("PlayerWin", SendMessageOptions.DontRequireReceiver);
        }
    }

    
}
