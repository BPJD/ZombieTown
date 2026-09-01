using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Player_Drone_Probe : MonoBehaviour
{
    public GameObject player;
    Player_Level playerRes;
    Unit_Status playerStat;
    GameObject building;
    Unit_Building_Status buildingStat;
    public Building_UI buildingUI;
    public Material[] materials;
    MeshRenderer rend;
    Transform tr;
    AudioSource soundPlayer;

    float rotSpd = 0f;

    bool gatherAble = false;
    int count;
    public int gatherTime = 20;
    bool isSoundPlaying = false;

    void Awake()
    {
        playerRes = player.GetComponentInChildren<Player_Level>();
        playerStat = player.GetComponent<Unit_Status>();
        rend = GetComponent<MeshRenderer>();
        tr = GetComponent<Transform>();
        soundPlayer = GetComponent<AudioSource>();
    }

    void Update()
    {
        tr.Rotate(Vector3.up * rotSpd * Time.deltaTime);
    }
    
    void Start()
    {
        StartCoroutine(Gather());
    }

    IEnumerator Gather()
    {
        while (true)
        {
            if(playerStat.player_state == Unit_Status.State.InBuilding)
            {
                gatherAble = true;
                building = GameObject.FindGameObjectWithTag("PlayerBuilding");
                buildingStat = building.GetComponent<Unit_Building_Status>();
            }
            else
            {
                gatherAble = false;
                building = null;
                buildingStat = null;
            }

            if (gatherAble && buildingStat != null)
            {

                if(buildingStat.farmable >= 1)
                {
                    if(count >= gatherTime)
                    {
                        count = 0;
                        buildingUI.SendMessage("FarmingComplete", SendMessageOptions.DontRequireReceiver);
                        rend.material = materials[0];
                        rotSpd = 0f;
                    }
                    else
                    {
                        count++;
                        rend.material = materials[1];
                        rotSpd = 180f;
                        if (!isSoundPlaying)
                        {
                            isSoundPlaying = true;
                            soundPlayer.Play();
                        }
                    }
                }
                else
                {
                    soundPlayer.Stop();
                    isSoundPlaying = false;
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }

    void ProbeGather(bool _gatherAble)
    {
        gatherAble = _gatherAble;
        if (gatherAble)
        {
            rend.material = materials[1];
        }
        else
        {
            rend.material = materials[0];
        }
    }

    void SetPlayerBuilding(GameObject _building)
    {
        building = _building;
        buildingStat = building.GetComponent<Unit_Building_Status>();
        count = 0;
    }

}
