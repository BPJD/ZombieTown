using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerActive : MonoBehaviour
{
    public Unit_Status playerStat;
    public GameObject upgradePanel;
    public GameObject dPad;
    public UI_Upgrade_Active upgrade_Active;
    void DPadActive()
    {
        if (playerStat.player_state == Unit_Status.State.InBuilding || upgradePanel.activeInHierarchy || upgrade_Active.isUpgradeAction)
        {
            dPad.SetActive(false);
        }
        else
        {
            dPad.SetActive(true);
        }
    }
}

