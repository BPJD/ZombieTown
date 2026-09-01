using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

public class Player_Move : MonoBehaviour //플레이어에 쓰는 이동 스크립트.
{
    NavMeshAgent navMesh;
    public GameObject destination;
    GameObject inBuild;
    Transform des_tr;
    Rigidbody rig;
    Unit_Status unit_stat;
    public GameObject manager;
    public GameObject building;
    public GameObject dPadManager;
    public GameObject droneProbe;
    CharacterController playerCollider;
    public Player_Drone_Repair repairDrone;
    Animator playerAnimator;
    Vector3 moveDir;


    void Awake()
    {
        navMesh = GetComponent<NavMeshAgent>();
        des_tr = destination.GetComponent<Transform>();
        rig = GetComponent<Rigidbody>();
        unit_stat = GetComponent<Unit_Status>();
        playerCollider = GetComponent<CharacterController>();
        playerAnimator = GetComponentInChildren<Animator>();
    }

    public void Move(Vector2 inputDirection) //모바일 조작
    {
        if (unit_stat.player_state == Unit_Status.State.Idle)
        {
            float x = inputDirection.x;
            float z = inputDirection.y;


            playerCollider.Move(moveDir * navMesh.speed * Time.deltaTime);

            MoveTo(new Vector3(x, 0f, z));

        }
    }

    void MoveTo(Vector3 dir)
    {
        moveDir = dir;
    }

    void MoveOrder(Vector3 _point) //플레이어를 움직이게 하는 메소드.
    {
        if(unit_stat.player_state == Unit_Status.State.Idle || unit_stat.player_state == Unit_Status.State.Move)
        {
            des_tr.position = _point;
            navMesh.SetDestination(des_tr.position);
            unit_stat.player_state = Unit_Status.State.Move;
            MoveAniPlay();
        }
    }
    public void StopOrder() //플레이어를 멈추게 하는 메소드.
    {
        navMesh.SetDestination(this.transform.position);
        unit_stat.player_state = Unit_Status.State.Idle;
        MoveAniStop();
    }

    void EntryOrder(GameObject _inBuild) //Building_Entry의 OnTriggerEnter와 같이 건물 입장 처리
    {
        MoveAniStop();
        navMesh.enabled = false;
        rig.isKinematic = true;
        inBuild = _inBuild;
        unit_stat.player_state = Unit_Status.State.InBuilding;
        dPadManager.SendMessage("DPadActive", SendMessageOptions.DontRequireReceiver);
        droneProbe.SendMessage("ProbeGather", true, SendMessageOptions.DontRequireReceiver);
        repairDrone.GetBuildingStat(inBuild);
        repairDrone.isRepairActive = true;
    }

    void SetBuilding(GameObject _building)
    {
        building = _building;
    }

    void OutOrder() // Building_Entry의 outOrder와 같이 건물 퇴장 처리
    {
        rig.isKinematic = false;
        inBuild.SendMessage("OutOrder", SendMessageOptions.DontRequireReceiver);
        navMesh.enabled = true;
        inBuild = null;
        unit_stat.player_state = Unit_Status.State.Idle;
        Invoke("Velocity_Reset", 0.3f);
        dPadManager.SendMessage("DPadActive", SendMessageOptions.DontRequireReceiver);
        droneProbe.SendMessage("ProbeGather", false, SendMessageOptions.DontRequireReceiver);
        repairDrone.isRepairActive = false;
    }

    void Velocity_Reset()
    {
        rig.linearVelocity = Vector3.zero;
    }

    void Down()
    {
        manager.SendMessage("Player_Dead", SendMessageOptions.DontRequireReceiver);
        unit_stat.atk = 0;
    }

    void MoveAniPlay()
    {
        playerAnimator.SetBool("IsWalking", true);
    }

    void MoveAniStop()
    {
        playerAnimator.SetBool("IsWalking", false);
    }
}
