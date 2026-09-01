using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building_Entry : MonoBehaviour //건물 입구에 쓰는 스크립트
{
    public GameObject parent;
    GameObject destBuilding;
    public Transform window;
    GameObject inUnit; //충돌한 오브젝트. 즉, 플레이어.
    GameObject gameManager;
    public Text buildingHP;
    bool inBuilding = false;
    Unit_Building_Status buildingStat;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController");
        buildingStat = GetComponentInParent<Unit_Building_Status>();
    }

    void Update()
    {
        
    }

    public void Player_Destination(GameObject _object) //건물이 내 목표 건물이 무엇인지를 받아오는 메소드. '목표건물이 아닌데 들어가지는 문제' 방지. MovePoint_Set에서 호출됨
    {
        destBuilding = _object;
    }

    void OnTriggerStay(Collider col)
    {
        if(col.CompareTag("Player") && destBuilding == parent) //플레이어가 목표 건물 입구에 닿은 경우
        {
            inUnit = col.gameObject;
            inUnit.SendMessage("EntryOrder", this.gameObject, SendMessageOptions.DontRequireReceiver); //플레이어가 건물에 들어가도록 플레이어에 호출
            inUnit.SendMessage("SetBuilding", parent, SendMessageOptions.DontRequireReceiver); //플레이어가 들어간 건물이 무엇인지 설정
            inUnit.transform.position = window.position; //플레이어를 창문 위치로
            destBuilding = null; // 목표건물 초기화
            inBuilding = true;
            gameManager.SendMessage("InBuilding", buildingStat.farmable, SendMessageOptions.DontRequireReceiver); //UI 띄우도록 GameManager에 호출, 매개변수 farmable는 건물 파밍 가능횟수
            gameManager.SendMessage("SetFarmingTime", buildingStat.time_for_farming, SendMessageOptions.DontRequireReceiver); //파밍에 필요한 시간 설정
            gameManager.SendMessage("SetRepairTime", buildingStat.time_for_repair, SendMessageOptions.DontRequireReceiver); //파밍에 필요한 시간 설정
            gameManager.SendMessage("SetRepairCost", buildingStat.repairCost, SendMessageOptions.DontRequireReceiver); //파밍에 필요한 시간 설정
            buildingStat.SendMessage("PlayerIsIn", SendMessageOptions.DontRequireReceiver);
        }
    }
    void OutOrder() //건물 나가는 것. 플레이어가 해당 메소드 호출
    {
        inUnit.transform.position = this.transform.position;
        inBuilding = false;
        buildingStat.SendMessage("PlayerIsOut", SendMessageOptions.DontRequireReceiver);
    }

    void OnGUI()
    {
        if (inBuilding)
        {
            buildingHP.text = "건물 HP : " + buildingStat.hp.ToString();
        }
    }
}
