using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightRotate_removed : MonoBehaviour
{
    Transform tr;
    public GameObject iconObj;
    public GameObject rotater;
    Image uiIcon;
    GameManage gameManage;
    Animator animator;
    GameObject spawnSystem;
    public Color finalDayLight;
    public Light nightLight;
    public Light dayLight;
    bool bossSpawned = false;
    public bool isDay = false;
    Rigidbody rg;
    public float debug_RefreshTime = 0.05f;

    public int dayCount = 1;

    // Start is called before the first frame update
    void Start()
    {
        rg = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        uiIcon = iconObj.GetComponent<Image>();
        gameManage = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManage>();
        spawnSystem = GameObject.FindGameObjectWithTag("Spawner");
        //animator.SetTrigger("DayToNight");
        StartCoroutine(LightRefresh());
    }

    // Update is called once per frame
    void Update()
    {
        if (tr.rotation.eulerAngles.x < 90 && !isDay) //낮이 될 때
        {
            Ani_NightToDay();
        }
        else if (tr.rotation.eulerAngles.x > 90 && isDay) //밤이 될 때
        {
            Ani_DayToNight();
        }
    }

    public void Ani_DayToNight() //밤이 되었음
    {
        isDay = false;
        rotater.SendMessage("dayChange", false, SendMessageOptions.DontRequireReceiver);
        uiIcon.sprite = Resources.Load<Sprite>("UI_Icon/icon_Night") as Sprite;
        dayLight.shadows = LightShadows.None;
        nightLight.shadows = LightShadows.Soft;

        if (dayCount == 7)
        {
            animator = GetComponent<Animator>();
            animator.enabled = true;
            if (!bossSpawned)
            {
                bossSpawned = true;
                spawnSystem.SendMessage("BossSpawn", SendMessageOptions.DontRequireReceiver);
            }
        }
        
    }

    public void Ani_NightToDay() //낮이 되었음
    {
        rotater.SendMessage("dayChange", true, SendMessageOptions.DontRequireReceiver);
        nightLight.shadows = LightShadows.None;
        dayLight.shadows = LightShadows.Soft;
        isDay = true;
        dayCount++;
        uiIcon.sprite = Resources.Load<Sprite>("UI_Icon/icon_Day") as Sprite;
        gameManage.SendMessage("GameLevel", SendMessageOptions.DontRequireReceiver);
        //animator.SetTrigger("DayToNight");
    }


    // 0 - 90 - 0 : 낮
    // 360 - 270 - 360 : 밤

    IEnumerator LightRefresh()
    {
        while (true)
        {
            tr.rotation = rotater.transform.rotation;
            yield return new WaitForSeconds(debug_RefreshTime);
        }
    }
}
