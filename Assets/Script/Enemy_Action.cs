using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Action : MonoBehaviour
{
    GameManage manager;
    GameObject manage;

    Rigidbody rig;
    Transform player;
    Transform tr;
    Unit_Status stat;
    NavMeshAgent navMesh;
    Collider col;
    public GameObject target;
    public GameObject zombieArm;
    public BoxCollider[] boxCols;
    float pathFindTime = 0.75f;
    bool isAttacking = false;
    Vector3 lastPlayerPosition;

    Animator animator;


    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        rig = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        stat = GetComponent<Unit_Status>();
        navMesh = GetComponent<NavMeshAgent>();
        col = GetComponent<Collider>();
        animator = GetComponent<Animator>();
        player = target.transform;
    }

    void Start()
    {
        lastPlayerPosition = player.position;
        navMesh.SetDestination(player.position);
        animator.SetBool("HasTarget", true);
        animator.SetFloat("MoveSpeed", navMesh.speed);
        StartCoroutine(ChasePlayer());
    }

    IEnumerator ChasePlayer()
    {
        while (true)
        {
            if(target != null && !isAttacking)
            {
                // 플레이어의 위치가 이전 위치와 다를 때만 SetDestination 호출
                if ((player.position - lastPlayerPosition).sqrMagnitude > 1f)
                {
                    lastPlayerPosition = player.position;
                    animator.SetBool("HasTarget", true);
                    navMesh.SetDestination(player.position);
                }

            }
            else
            {
                if (stat.player_state != Unit_Status.State.Dead)
                {
                    animator.SetBool("HasTarget", false);
                    navMesh.ResetPath();
                }
            }
            yield return new WaitForSeconds(pathFindTime);
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
        zombieArm.SetActive(false);
        boxCols[0].enabled = true;
        boxCols[1].enabled = true;
        boxCols[2].enabled = true;

        animator.SetInteger("RandomMotion", Random.Range(0, 3));
        animator.SetBool("IsDied", true);
        animator.SetTrigger("Die");
    }

    void MoveSpeedDown()
    {
        pathFindTime = 2f;
        navMesh.speed = 2f;
        animator.SetFloat("MoveSpeed", navMesh.speed);
    }



    
}
