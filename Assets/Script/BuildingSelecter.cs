using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSelecter : MonoBehaviour
{
    GameObject selectedBuilding;
    public GameObject buildingSelectUI;
    Vector3 buttonPosition = new Vector3(0, 20, 0);
    public Building_UI gameManager;

    /*
    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Building" && selectedBuilding != col.gameObject)
        {
            selectedBuilding = col.gameObject;
            //gameManager.RecieveSelectedBuilding(selectedBuilding);
            gameManager.BuildingUI(buildingSelectUI, true);
        }
    }
    */
    
    void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Building") && selectedBuilding != col.gameObject)
        {
            selectedBuilding = col.gameObject;
            gameManager.RecieveSelectedBuilding(selectedBuilding);
            gameManager.BuildingUI(buildingSelectUI, true);
        }
    }
    

    void OnTriggerExit(Collider col)
    {
        if(col.gameObject == selectedBuilding)
        {
            selectedBuilding = null;
            //gameManager.RecieveSelectedBuilding(selectedBuilding);
            gameManager.BuildingUI(buildingSelectUI, false);
        }
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
}
