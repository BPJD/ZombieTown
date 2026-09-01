using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatForDebug : MonoBehaviour
{
    public Unit_Status playerStat;
    public Player_Level playerRes;
    public Upgrade_StatusManager upgradeStat;
    public Card_CallOut cardCall;

    // Start is called before the first frame update
    void Start()
    {
        playerStat.maxHp = 9999999;
        playerStat.hp = 9999999;
        playerRes.exp_revision = 0f;
        playerRes.ammoMax_revision = 100f;
        playerRes.foodMax_revision = 100f;
        playerRes.partMax_revision = 100f;
        playerRes.ammoUp_revision = 100f;
        playerRes.partUp_revision = 100f;
        playerRes.foodUp_revision = 100f;
        upgradeStat.timeDecrease = 0.1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            cardCall.CardDraw();
        }
    }
}
