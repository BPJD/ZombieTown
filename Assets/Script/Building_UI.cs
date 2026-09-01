using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class Building_UI : MonoBehaviour //게임매니저에 쓰는 스크립트
{
    public GameObject selectedBuilding; //누른 건물
    GameObject destBuilding; //목표 건물
    Unit_Building_RevisionManager revisionManager;
    bool ui_opened = false;
    public GameObject buildingSelectUI;
    public GameObject inBuildingUI;
    public GameObject inBuildingActionUI;
    public GameObject weaponButton;
    public GameObject upgradeUI;
    public GameObject selectedBuildingButton;
    GameObject player;
    Player_Level playerRes;
    public GameObject player_act;
    Unit_Status stat;
    UI_Upgrade_Active equip;

    public GameObject droneProbe;

    int farmable_count;
    public GameObject ui_Farmable;
    public Text txt_Farmable;
    public Text txt_RepairCost;
    float remain_for_farming;
    float remain_for_repair;
    int cost_for_repair;
    float timer;
    bool player_is_action = false;
    public Text txt_Action_remainTime;
    string action;

    SoundPlayer_UI soundPlayer;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        stat = player.GetComponent<Unit_Status>();
        playerRes = player_act.GetComponent<Player_Level>();
        equip = GetComponentInChildren<UI_Upgrade_Active>();
        revisionManager = GetComponent<Unit_Building_RevisionManager>();
        soundPlayer = GameObject.FindGameObjectWithTag(SoundPlayerData.soundPlayerTag).GetComponent<SoundPlayer_UI>();
    }

    void Update()
    {
        if (ui_opened && stat.player_state != Unit_Status.State.InBuilding)
        {
            BuildingUI(buildingSelectUI, true);
            //
            ///';
            ///selectedBuildingButton.transform.position = Camera.main.WorldToScreenPoint(selectedBuilding.transform.position) + new Vector3(0, 20, 0);
        }
        /*
        if (Input.GetMouseButtonDown(0) && !equip.isUpgradeAction && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 8 ))
            {
                if(hit.collider.tag == "Building")
                {
                    selectedBuilding = hit.collider.gameObject; // 레이캐스트에 감지된 건물을 선택
                    if (!ui_opened && stat.player_state != Unit_Status.State.InBuilding)
                    {
                        BuildingUI(buildingSelectUI, true);
                        selectedBuildingButton.transform.position = Camera.main.WorldToScreenPoint(selectedBuilding.transform.position) + new Vector3(0, 20, 0);
                    }
                }
            }
        }
        */


        if (player_is_action)
        {
            timer -= Time.deltaTime;
            txt_Action_remainTime.text = "남은 시간 : " + timer.ToString("F1");
            if(timer <= 0f)
            {
                BuildingUI(inBuildingUI, true);
                BuildingUI(inBuildingActionUI, false);
                weaponButton.SetActive(true);
                player_act.SendMessage("Farming", false, SendMessageOptions.DontRequireReceiver); // Player_Action - 다시 공격할 수 있게 함
                player_is_action = false;

                switch (action)
                {
                    case "Farming":
                        FarmingComplete();
                        break;
                    case "Repair":
                        RepairComplete();
                        break;
                }
            }
        }
    }

    public void RecieveSelectedBuilding(GameObject _building)
    {
        selectedBuilding = _building;
    }

    void FarmingComplete()
    {
        farmable_count--;
        destBuilding.SendMessage("Farmed", SendMessageOptions.DontRequireReceiver);
        player_act.SendMessage("Farmed", SendMessageOptions.DontRequireReceiver); // Player_Level - 자원을 얻음

        if (farmable_count >= 1)
        {
            txt_Farmable.text = "조사 가능 횟수 : " + farmable_count;
            ui_Farmable.SetActive(true);
        }
        else
        {
            ui_Farmable.SetActive(false);
        }
        soundPlayer.UIAudioPlay(SoundPlayerData.farmingComplete);
    }

    void RepairComplete()
    {
        destBuilding.SendMessage("Repair", SendMessageOptions.DontRequireReceiver);
        playerRes.res_Part -= cost_for_repair;
        soundPlayer.UIAudioPlay(SoundPlayerData.repaired);
    }

    public void BuildingUI(GameObject _ui, bool _open) //매개변수 _ui는 어떤 ui를 활성/비활성 하는지, _open 은 ui를 보이게/안보이게 하는지.
    {
        _ui.SetActive(_open);
        ui_opened = _open;
        //_ui.transform.position = Camera.main.WorldToScreenPoint(selectedBuilding.transform.position) + new Vector3(0, 80, 0);
    }

    public void Entry_Order_Clicked() // 건물에 들어가기 버튼 눌렀을 때
    {
        destBuilding = selectedBuilding; // 선택한 건물을 목표 건물로 설정
        BuildingUI(buildingSelectUI, false); // UI 닫기
        destBuilding.SendMessage("SetMovePoint", SendMessageOptions.DontRequireReceiver);
        soundPlayer.UIAudioPlay(SoundPlayerData.buttonClicked);
        //선택 건물, 목표 건물. 이 두 가지로 이동지점 설정 절차를 늘린 것은 '이동시키고 다른 건물을 누르면 목표 건물에 안들어가지는 문제' 를 방지하기 위함.
    }

    public void Out_Order_Clicked() // 건물에서 나오기 버튼 눌렀을 때
    {
        player.SendMessage("OutOrder", SendMessageOptions.DontRequireReceiver);
        BuildingUI(inBuildingUI, false);
        soundPlayer.UIAudioPlay(SoundPlayerData.buildingOut);
    }

    void InBuilding(int _farmable) //플레이어가 건물에 들어갔을 때
    {
        if (_farmable >= 1)
        {
            farmable_count = _farmable;
            txt_Farmable.text = "조사 가능 횟수 : " + _farmable;
            ui_Farmable.SetActive(true);
            droneProbe.SendMessage("SetPlayerBuilding", destBuilding, SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            ui_Farmable.SetActive(false);
        }
        BuildingUI(inBuildingUI, true);
        BuildingUI(buildingSelectUI, false);
        soundPlayer.UIAudioPlay (SoundPlayerData.buildingIn);

    }

    void SetFarmingTime(float _timeForAction)
    {
        remain_for_farming = _timeForAction * revisionManager.buildingLootTime;
    }
    void SetRepairTime(float _timeForAction)
    {
        remain_for_repair = _timeForAction;
    }
    void SetRepairCost(int _cost)
    {
        cost_for_repair = _cost;
        txt_RepairCost.text = "수리 비용 : " + cost_for_repair;
    }

    public void Farming_Clicked()
    {
        action = "Farming";
        timer = remain_for_farming;
        player_is_action = true;
        player_act.SendMessage("Farming", true, SendMessageOptions.DontRequireReceiver);
        BuildingUI(inBuildingActionUI, true);
        BuildingUI(inBuildingUI, false);
        weaponButton.SetActive(false);
        upgradeUI.SetActive(false);
        soundPlayer.UIAudioPlay(SoundPlayerData.farmingStart);
    }
    public void Repair_Clicked()
    {
        if(playerRes.res_Part >= cost_for_repair)
        {
            action = "Repair";
            timer = remain_for_repair;
            player_is_action = true;
            player_act.SendMessage("Farming", true, SendMessageOptions.DontRequireReceiver);
            BuildingUI(inBuildingActionUI, true);
            BuildingUI(inBuildingUI, false);
            weaponButton.SetActive(false);
            upgradeUI.SetActive(false);
            soundPlayer.UIAudioPlay(SoundPlayerData.buttonClicked);
        }
        else
        {
            soundPlayer.UIAudioPlay(SoundPlayerData.buttonDenied);
        }
    }

    public void Action_Cancel()
    {
        BuildingUI(inBuildingUI, true);
        BuildingUI(inBuildingActionUI, false);
        weaponButton.SetActive(true);
        player_act.SendMessage("Farming", false, SendMessageOptions.DontRequireReceiver);
        player_is_action = false;
        soundPlayer.UIAudioPlay(SoundPlayerData.upgradecancel);
    }

}
