using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePoint_Set : MonoBehaviour //건물에 쓰는 스크립트
{
    GameObject player;
    public Transform entryPoint;
    Building_Entry buildingEntry;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        buildingEntry = GetComponentInChildren<Building_Entry>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetMovePoint() //건물로 이동을 결정했을 때 호출됨.
    {
        player.SendMessage("MoveOrder", entryPoint.position, SendMessageOptions.DontRequireReceiver);
        buildingEntry.Player_Destination(this.gameObject);
    }


}
