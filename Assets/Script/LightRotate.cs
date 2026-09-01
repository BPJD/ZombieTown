using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightRotate : MonoBehaviour
{
    GameObject clock;
    public RectTransform handTr;
    LightRotater handOfClock;
    Animator thisAnimator;
    Animator clockAnimator;
    GameManage manager;
    public UI_NotificationSystem notification;
    public MobSpawn spawner;
    public enum State { Sunrise, Sunset, Night, Dawn, Final};
    State time = State.Sunrise;
    public int dayCount = 1;
    int weekCount = 1;
    public Text dayText;
    public bool isDay = true;
    public bool isBossNight = false;
    Player_Level playerLevel;
    public Card_CallOut cardCaller;
    public int cardDrawCount = 30;
    BGMPlayer bgmPlayer;

    // Start is called before the first frame update
    void Start()
    {
        bgmPlayer = GetComponent<BGMPlayer>();
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManage>();
        isDay = true;
        clock = GameObject.FindGameObjectWithTag("Clock");
        handOfClock = clock.GetComponentInChildren<LightRotater>();
        thisAnimator = GetComponent<Animator>();
        clockAnimator = clock.GetComponent<Animator>();
        playerLevel = GameObject.Find("PlayerMesh").GetComponent<Player_Level>();
        StartCoroutine(TimeCheck());
    }

    void GameResumed()
    {
        time = State.Night;
    }

    IEnumerator TimeCheck()
    {
        while (true)
        {
            switch (time)
            {
                case State.Sunrise://오후
                    if(handTr.rotation.eulerAngles.z <= 90)
                    {
                        time = State.Sunset;
                        bgmPlayer.MusicPlay(0);
                    }
                    break;
                case State.Sunset://해가 졌을때
                    if(handTr.rotation.eulerAngles.z >= 180)
                    {
                        spawner.mob_spawnAble = true;
                        if(weekCount == 7)
                        {
                            isDay = false;
                            notification.TextOutPut(0);
                            handOfClock.dayChange(isDay, weekCount);
                            time = State.Night;
                            spawner.BossSpawn();
                            thisAnimator.SetTrigger("FinalNight");
                            clockAnimator.SetTrigger("DayToNight");
                            weekCount = 0;
                            isBossNight = true;
                            bgmPlayer.MusicPlay(1);
                        }
                        else
                        {
                            isDay = false;
                            handOfClock.dayChange(isDay, weekCount);
                            time = State.Night;
                            thisAnimator.SetTrigger("DayToNight");
                            clockAnimator.SetTrigger("DayToNight");
                            isBossNight = false;
                            bgmPlayer.MusicPlay(2);
                        }
                    }
                    break;
                case State.Night: //자정~새벽
                    if(handTr.rotation.eulerAngles.z <= 90)
                    {
                        time = State.Dawn;
                        bgmPlayer.MusicPlay(0);
                    }
                    break;
                case State.Dawn: //해가 뜰 때
                    if(handTr.rotation.eulerAngles.z >= 180)
                    {
                        manager.GameLevelUp();
                        isDay = true;
                        isBossNight = false;
                        handOfClock.dayChange(isDay, weekCount);
                        time = State.Sunrise;
                        thisAnimator.SetTrigger("NightToDay");
                        clockAnimator.SetTrigger("NightToDay");
                        dayCount++;
                        weekCount++;
                        dayText.text = dayCount + "일차";
                        if(weekCount == 7)
                        {
                            dayText.color = Color.red;
                        }
                        else
                        {
                            dayText.color = Color.white;
                            manager.enemyHpRevision += 0.8f; //적 체력 보정치 증가
                        }
                        bgmPlayer.MusicPlay(0);
                    }
                    break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }



}
