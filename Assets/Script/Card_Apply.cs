using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Card_Apply : MonoBehaviour
{
    Card_InteractionEvent cardData;
    GameObject playerStatObj;
    GameObject playerMesh;
    Player_Level player_resources;
    Unit_Status player_status;
    NavMeshAgent playerNav;
    int selectedCardArray;
    GameObject gameManager;
    Player_Equip player_weapon;
    public Upgrade_StatusManager upgrade_revision;
    MobSpawn mobSpawner;
    GameObject sun;
    LightRotate sunLight;
    Card_DatabaseManager card_database;
    Player_Drone_Rotate attackerRotate;
    float default_speed;
    float speed_revision;

    public GameObject playerDrone;
    public GameObject[] attackerDronesObj;
    public Player_Drone_Attacker[] attackerDrones;
    public Player_Drone_Bomb bombDrone;
    public Player_Drone_Satelite sateliteDrone;
    public Player_Drone_Repair repairDrone;
    public Player_Drone_Probe probeDrone;


    SoundPlayer_UI uiSoundPlayer;

    // Start is called before the first frame update
    void Start()
    {
        uiSoundPlayer = GameObject.FindGameObjectWithTag(SoundPlayerData.soundPlayerTag).GetComponent<SoundPlayer_UI>();
        card_database = GetComponent<Card_DatabaseManager>();
        playerMesh = GameObject.Find("PlayerMesh");
        cardData = GetComponent<Card_InteractionEvent>();
        playerStatObj = GameObject.FindGameObjectWithTag("Player");
        player_resources = playerMesh.GetComponent<Player_Level>();
        player_status = playerStatObj.GetComponent<Unit_Status>();
        playerNav = playerStatObj.GetComponent<NavMeshAgent>();
        gameManager = GameObject.FindGameObjectWithTag("GameController");
        player_weapon = playerMesh.GetComponent<Player_Equip>();
        mobSpawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<MobSpawn>();
        attackerRotate = attackerDrones[0].GetComponentInParent<Player_Drone_Rotate>();

        sun = GameObject.FindGameObjectWithTag("Sun");
        sunLight = sun.GetComponent<LightRotate>();

        default_speed = playerNav.speed;
    }

    public void ApplyCardData(int _card)
    {
        selectedCardArray = _card;
        uiSoundPlayer.UIAudioPlay(SoundPlayerData.buttonClicked);
        switch (cardData.card.cards[_card].cardType)
        {
            case Card.Type.player://플레이어
                PlayerCardApplied(cardData.card.cards[_card].cardID.Substring(1));
                break;

            case Card.Type.resource://자원
                ResourceCardApplied(cardData.card.cards[_card].cardID.Substring(1));
                break;

            case Card.Type.weapon://무기
                WeaponCardApplied(cardData.card.cards[_card].cardID.Substring(1));
                break;

            case Card.Type.upgrade://업그레이드
                UpgradeCardApplied(cardData.card.cards[_card].cardID.Substring(1));
                break;

            case Card.Type.building://건물
                BuildingCardApplied(cardData.card.cards[_card].cardID.Substring(1));
                break;
                
            case Card.Type.drone://드론
                DroneCardApplied(cardData.card.cards[_card].cardID.Substring(1));
                break;

            case Card.Type.spawner://스포너
                SpawnerCardApplied(cardData.card.cards[_card].cardID.Substring(1));
                break;

            case Card.Type.timer://시계
                TimerCardApplied(cardData.card.cards[_card].cardID.Substring(1));
                break;
        }

        switch (selectedCardArray)
        {
            case 0:
                card_database.RemoveCard(cardData.card.cardLeftID);
                break;

            case 1:
                card_database.RemoveCard(cardData.card.cardCenterID);
                break;

            case 2:
                card_database.RemoveCard(cardData.card.cardRightID);
                break;
        }

        //Debug.Log(card_database.overlapList.Count + " : CardSystem");

        player_resources.remainCardPoint--;
        if(player_resources.remainCardPoint >= 1)
        {
            Invoke("Draw", 0.1f);
        }
    }

    void Draw()
    {
        this.SendMessage("CardDraw", SendMessageOptions.DontRequireReceiver);
    }

    void PlayerCardApplied(string _id) // ID 앞자리 0
    {
        switch (_id)
        {
            case "00":
                speed_revision += cardData.card.cards[selectedCardArray].cardRef_a;
                playerNav.speed = default_speed * (speed_revision + 1);
                break;
            case "01":
                player_resources.HpRevisionUp(cardData.card.cards[selectedCardArray].cardRef_a);
                break;
            case "02":
                //player_status.NanoHealerActive();
                player_resources.hpHealPer += player_resources.hpHealPer * cardData.card.cards[selectedCardArray].cardRef_a;
                break;
            case "03":
                player_resources.exp_revision += cardData.card.cards[selectedCardArray].cardRef_a;
                player_resources.HpRevisionUp(-cardData.card.cards[selectedCardArray].cardRef_b);
                break;
            case "04":
                player_resources.exp_revision -= cardData.card.cards[selectedCardArray].cardRef_b;
                player_resources.HpRevisionUp(cardData.card.cards[selectedCardArray].cardRef_a);
                break;
            case "05":

                break;
        }
    }

    void ResourceCardApplied(string _id) // ID 앞자리 1
    {
        switch (_id)
        {
            case "00":
                player_resources.foodMax_revision += cardData.card.cards[selectedCardArray].cardRef_a;
                player_resources.partMax_revision += cardData.card.cards[selectedCardArray].cardRef_a;
                player_resources.ammoMax_revision += cardData.card.cards[selectedCardArray].cardRef_a;
                player_resources.MaxResourceSetup();
                break;
            case "01":
                player_resources.rand_foodRate += (int)cardData.card.cards[selectedCardArray].cardRef_a * 100;
                break;
            case "02":
                player_resources.partUp_revision += cardData.card.cards[selectedCardArray].cardRef_a;
                player_resources.MaxResourceSetup();
                break;
            case "03":
                player_resources.foodUp_revision += cardData.card.cards[selectedCardArray].cardRef_a;
                player_resources.partUp_revision += cardData.card.cards[selectedCardArray].cardRef_a;
                player_resources.ammoUp_revision += cardData.card.cards[selectedCardArray].cardRef_a;
                gameManager.GetComponent<Unit_Building_RevisionManager>().buildingLootTime += (cardData.card.cards[selectedCardArray].cardRef_b);
                break;
            case "04":
                player_resources.food_down_per_second *= cardData.card.cards[selectedCardArray].cardRef_a;
                break;
        }

    }

    void WeaponCardApplied(string _id) // ID 앞자리 2
    {
        switch (_id)            //weaponCode : Pistol, Revolver, SMG, SR, AR
        {                       //weaponStat : Range, AtkSpd, Atk, AmmoCost
            case "00":
                player_weapon.WeaponStatusSet(4, 0, -cardData.card.cards[selectedCardArray].cardRef_a);
                player_weapon.WeaponStatusSet(4, 1, -cardData.card.cards[selectedCardArray].cardRef_b);
                break;
            case "01":
                player_weapon.WeaponStatusSet(4, 0, cardData.card.cards[selectedCardArray].cardRef_a);
                break;
            case "02":
                player_weapon.WeaponStatusSet(4, 2, cardData.card.cards[selectedCardArray].cardRef_a);
                break;
            case "03":
                player_weapon.WeaponStatusSet(2, 3, cardData.card.cards[selectedCardArray].cardRef_a);
                player_weapon.WeaponStatusSet(2, 2, cardData.card.cards[selectedCardArray].cardRef_b);
                break;
            case "04":
                player_weapon.WeaponStatusSet(2, 0, cardData.card.cards[selectedCardArray].cardRef_a);
                break;
            case "05":
                player_weapon.WeaponStatusSet(3, 0, -cardData.card.cards[selectedCardArray].cardRef_a);
                player_weapon.WeaponStatusSet(3, 1, -cardData.card.cards[selectedCardArray].cardRef_b);
                break;
            case "06":
                player_weapon.WeaponStatusSet(3, 0, cardData.card.cards[selectedCardArray].cardRef_a);
                player_weapon.WeaponStatusSet(3, 2, cardData.card.cards[selectedCardArray].cardRef_b);
                break;
            case "07":
                player_weapon.WeaponStatusSet(0, 1, -cardData.card.cards[selectedCardArray].cardRef_a);
                break;
            case "08":
                player_weapon.WeaponStatusSet(0, 2, cardData.card.cards[selectedCardArray].cardRef_a);
                break;
            case "09":
                player_weapon.WeaponStatusSet(1, 1, -cardData.card.cards[selectedCardArray].cardRef_a);
                break;
            case "10":
                player_weapon.WeaponStatusSet(1, 2, cardData.card.cards[selectedCardArray].cardRef_a);
                break;
        }

    }

    void UpgradeCardApplied(string _id) // ID 앞자리 3
    {
        switch (_id)
        {
            case "00":
                upgrade_revision.timeDecrease += -cardData.card.cards[selectedCardArray].cardRef_a;
                break;
            case "01":
                upgrade_revision.reqPartDecrease += -cardData.card.cards[selectedCardArray].cardRef_a;
                break;
            case "02":
                upgrade_revision.ammoIncrease += cardData.card.cards[selectedCardArray].cardRef_a;
                break;
        }

    }

    void BuildingCardApplied(string _id) // ID 앞자리 4
    {
        switch (_id)
        {
            case "00":
                gameManager.GetComponent<Unit_Building_RevisionManager>().buildingLootTime += (-cardData.card.cards[selectedCardArray].cardRef_a);
                break;
            case "01":
                gameManager.GetComponent<Unit_Building_RevisionManager>().buildingRepair += (cardData.card.cards[selectedCardArray].cardRef_a);
                break;
        }

    }

    void DroneCardApplied(string _id) // ID 앞자리 5
    {
        switch (_id)
        {
            case "00":
                attackerDronesObj[1].SetActive(true);
                break;
            case "01":
                attackerDrones[0].scoutRevision += cardData.card.cards[selectedCardArray].cardRef_a;
                attackerDrones[0].transform.localPosition += new Vector3(attackerDrones[0].transform.localPosition.x * attackerDrones[0].scoutRevision, 0f, 0f);
                attackerDrones[1].transform.localPosition += new Vector3(attackerDrones[1].transform.localPosition.x * attackerDrones[0].scoutRevision, 0f, 0f);
                break;
            case "02":
                attackerDronesObj[0].GetComponent<SphereCollider>().radius *= (1f + cardData.card.cards[selectedCardArray].cardRef_a);
                attackerDronesObj[1].GetComponent<SphereCollider>().radius *= (1f + cardData.card.cards[selectedCardArray].cardRef_a);
                break;
            case "03":
                attackerDrones[0].atkRevision *= (1f + cardData.card.cards[selectedCardArray].cardRef_a);
                attackerDrones[1].atkRevision *= (1f + cardData.card.cards[selectedCardArray].cardRef_a);
                attackerDrones[0].atkSpd *= (1f + cardData.card.cards[selectedCardArray].cardRef_b);
                attackerDrones[1].atkSpd *= (1f + cardData.card.cards[selectedCardArray].cardRef_b);
                break;
            case "04":
                
                attackerRotate.speed *= (1f + cardData.card.cards[selectedCardArray].cardRef_a);
                break;
            case "05":
                probeDrone.gatherTime = Mathf.RoundToInt(probeDrone.gatherTime * cardData.card.cards[selectedCardArray].cardRef_a);
                break;
            case "06":
                sateliteDrone.weaponType = 1;
                break;
            case "07":
                sateliteDrone.accuracyRevision -= cardData.card.cards[selectedCardArray].cardRef_a;
                sateliteDrone.accuracy[0] = sateliteDrone.accuracyDefault[0] * sateliteDrone.accuracyRevision;
                sateliteDrone.accuracy[1] = sateliteDrone.accuracyDefault[1] * sateliteDrone.accuracyRevision;
                break;
            case "08":
                sateliteDrone.reloadTimeRevision -= cardData.card.cards[selectedCardArray].cardRef_a;
                sateliteDrone.reloadTimes[0] = sateliteDrone.reloadTimesDefault[0] * sateliteDrone.reloadTimeRevision;
                sateliteDrone.reloadTimes[1] = sateliteDrone.reloadTimesDefault[1] * sateliteDrone.reloadTimeRevision;

                sateliteDrone.accuracyRevision += cardData.card.cards[selectedCardArray].cardRef_b;
                sateliteDrone.accuracy[0] = sateliteDrone.accuracyDefault[0] * sateliteDrone.accuracyRevision;
                sateliteDrone.accuracy[1] = sateliteDrone.accuracyDefault[1] * sateliteDrone.accuracyRevision;
                break;
            case "09":
                sateliteDrone.atkRevision += cardData.card.cards[selectedCardArray].cardRef_a;
                sateliteDrone.range += sateliteDrone.range * cardData.card.cards[selectedCardArray].cardRef_b;
                break;
            case "10":
                bombDrone.damageRevision += cardData.card.cards[selectedCardArray].cardRef_a;
                bombDrone.reloadTimes[0] += bombDrone.reloadTimesDefault[0] * cardData.card.cards[selectedCardArray].cardRef_b;
                bombDrone.reloadTimes[1] += bombDrone.reloadTimesDefault[1] * cardData.card.cards[selectedCardArray].cardRef_b;
                break;
            case "11":
                bombDrone.weaponType = 1;
                break;
            case "12":
                bombDrone.reloadTimes[0] -= bombDrone.reloadTimesDefault[0] * cardData.card.cards[selectedCardArray].cardRef_a;
                bombDrone.damageRevision -= cardData.card.cards[selectedCardArray].cardRef_b;
                break;
            case "13":
                repairDrone.repairPerSec += (int)(repairDrone.repairPerSec * cardData.card.cards[selectedCardArray].cardRef_a);
                break;
            case "14":
                repairDrone.repairMaxHp += (int)cardData.card.cards[selectedCardArray].cardRef_a;
                break;
        }
    }

    void SpawnerCardApplied(string _id) // ID 앞자리 6
    {
        switch (_id)
        {
            case "00":
                mobSpawner.day_spawn_per_second -= mobSpawner.day_spawn_per_second * cardData.card.cards[selectedCardArray].cardRef_a;
                mobSpawner.night_spawn_per_second -= mobSpawner.night_spawn_per_second * cardData.card.cards[selectedCardArray].cardRef_a;
                player_resources.exp_revision += cardData.card.cards[selectedCardArray].cardRef_b;
                break;
            case "01":
                mobSpawner.day_spawn_per_second += mobSpawner.day_spawn_per_second * cardData.card.cards[selectedCardArray].cardRef_a;
                mobSpawner.night_spawn_per_second += mobSpawner.night_spawn_per_second * cardData.card.cards[selectedCardArray].cardRef_a;

                player_resources.foodMax_revision -= cardData.card.cards[selectedCardArray].cardRef_b;
                player_resources.partMax_revision -= cardData.card.cards[selectedCardArray].cardRef_b;
                player_resources.ammoMax_revision -= cardData.card.cards[selectedCardArray].cardRef_b;
                player_resources.MaxResourceSetup();
                break;
        }

    }

    void TimerCardApplied(string _id)
    {

    }


}
