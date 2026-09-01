using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManage : MonoBehaviour
{
    public GameObject win_UI;
    public GameObject dead_UI;
    public GameObject dPad;
    public GameObject game_building_UI;
    public GameObject game_weapon_UI;
    public GameObject toMainConfirm_UI;
    public GameObject game_pause_UI;
    int score;
    public Text txt_Score;
    public Text txt_Time;
    public Text win_txt_Score;
    public Text win_txt_Time;
    public GameObject players;
    public bool isGameOver = false;

    LightRotate dayCounter;
    Building_UI ui;

    public int enemyHpUpTotal;
    public float enemyAtkUpTotal;
    int enemyHpUp = 30; //체력 증가량
    float enemyAtkUp = 0.5f; //공격력 증가량
    public float enemyHpRevision = 0f; //체력 보정치
    public int enemyStatUpDay = 2; //며칠마다 스탯이 올라가느냐
    int enemyStatUpCounter = 0;
    UI_NotificationSystem notification;

    public int curSceneCount;

    public int farmingExp = 15;
    public int killingExp = 3;
    public int expUpDay = 3; //며칠마다 경험치 획득량이 증가하느냐
    int expUpCounter = 0;
    float killingRevision = 0.5f;
    float farmingRevision = 0f;


    public Text debug_gamelevel;


    void Awake()
    {
        Application.targetFrameRate = 40;
    }

    // Start is called before the first frame update
    void Start()
    {
        dayCounter = GameObject.FindGameObjectWithTag("Sun").GetComponent<LightRotate>();
        ui = GetComponent<Building_UI>();
        players = GameObject.FindGameObjectWithTag("Player");
        notification = GetComponentInChildren<UI_NotificationSystem>();

        killingExp += (int)(killingExp * killingRevision);
    }

    public void Restart() //게임 재시작
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(curSceneCount);
    }

    public void ToMainConfirmed() //돌아갈건지 확인
    {
        toMainConfirm_UI.SetActive(true);
    }
    public void ToMainNotConfirmed() //안돌아감
    {
        toMainConfirm_UI.SetActive(false);
    }
    public void ToMainClicked() //돌아감
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void PauseClicked()
    {
        Time.timeScale = 0f;
        game_pause_UI.SetActive(true);
    }
    public void ResumeClicked()
    {
        Time.timeScale = 1f;
        game_pause_UI.SetActive(false);
    }

    void Player_Dead()
    {
        players.SendMessage("LevelReset", SendMessageOptions.DontRequireReceiver);
        isGameOver = true;
        Time.timeScale = 0.5f;
        txt_Time.text = "당신은 " + dayCounter.dayCount + " 일 동안";
        txt_Score.text = score.ToString() + " 마리의 좀비를 죽였습니다.";
        ui.enabled = false;
        game_weapon_UI.SetActive(false);
        game_building_UI.SetActive(false);
        dead_UI.SetActive(true);
        dPad.SetActive(false);
    }

    void PlayerWin()
    {
        Time.timeScale = 0f;
        win_txt_Time.text = "당신은 " + dayCounter.dayCount + " 일 동안";
        win_txt_Score.text = score.ToString() + " 마리의 좀비를 죽였습니다.";
        ui.enabled = false;
        game_weapon_UI.SetActive(false);
        game_building_UI.SetActive(false);
        win_UI.SetActive(true);
        dPad.SetActive(false);
    }

    void Player_TargetKilled()
    {
        score++;
    }

    
    public void GameLevelUp() //일차 증가했을 때
    {
        enemyStatUpCounter++;
        expUpCounter++;
        if (enemyStatUpCounter >= enemyStatUpDay) //적 스탯 증가
        {
            enemyAtkUpTotal += enemyAtkUp;
            enemyHpUpTotal += enemyHpUp;
            enemyStatUpCounter = 0;
            notification.TextOutPut(1);
        }

        if(expUpCounter >= expUpDay)
        {
            farmingRevision += 0.4f;
            killingRevision += 0.4f;
            farmingExp += (int)(farmingExp * farmingRevision);
            killingExp += (int)(killingExp * killingRevision);
            expUpCounter = 0;
        }
    }
    

    
}
