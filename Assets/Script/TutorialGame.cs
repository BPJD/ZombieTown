using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialGame : MonoBehaviour
{
    [TextArea] public List<string> tutorialText;

    int textCount = 0;
    public GameObject tutorialUI;
    public Text uiText;
    public GameObject clock;
    public GameObject gameManager;
    public GameObject player;
    public GameObject player_buildingSelecter;
    public GameObject upgradeButton;
    public GameObject smg;
    public GameObject light;
    public GameObject repairButton;


    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;
        uiText.text = tutorialText[0];
        StartCoroutine(StepCheck());
    }

    public void NextClicked()
    {
        GameResume(true);
        tutorialUI.SetActive(false);
        textCount++;
    }

    void GameResume(bool _isResume)
    {
        if (_isResume)
        {
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0f;
            tutorialUI.SetActive(true);
        }
    }

    IEnumerator StepCheck()
    {
        while (true)
        {
            switch (textCount)
            {
                case 1:
                    uiText.text = tutorialText[textCount];
                    clock.GetComponent<Rigidbody2D>().angularVelocity = 0f;
                    GameResume(false);
                    break;
                case 2:
                    if(GameObject.FindGameObjectWithTag("Dead") != null)
                    {
                        uiText.text = tutorialText[textCount];
                        clock.transform.rotation = Quaternion.Euler(0f, 0f, 270f);
                        GameResume(false);
                    }
                    break;
                case 3:
                    uiText.text = tutorialText[textCount];
                    player_buildingSelecter.SetActive(true);
                    clock.transform.rotation = Quaternion.Euler(0f, 0f, 220f);
                    GameResume(false);
                    break;
                case 4:
                    if (GameObject.FindGameObjectWithTag("PlayerBuilding") != null)
                    {
                        uiText.text = tutorialText[textCount];
                        clock.transform.rotation = Quaternion.Euler(0f, 0f, 220f);
                        GameResume(false);
                    }
                    break;
                case 5:
                    if (player.GetComponentInChildren<Player_Level>().res_Part >= 16)
                    {
                        uiText.text = tutorialText[textCount];
                        clock.transform.rotation = Quaternion.Euler(0f, 0f, 220f);
                        GameResume(false);
                    }
                    break;
                case 6:
                    uiText.text = tutorialText[textCount];
                    player.GetComponentInChildren<Player_Level>().res_Part = 150;
                    player.GetComponentInChildren<Player_Level>().res_Ammo = 150;
                    clock.transform.rotation = Quaternion.Euler(0f, 0f, 220f);
                    upgradeButton.SetActive(true);
                    GameResume(false);
                    break;
                case 7:
                    if (smg.gameObject.activeInHierarchy)
                    {
                        uiText.text = tutorialText[textCount];
                        repairButton.SetActive(true);
                        clock.transform.rotation = Quaternion.Euler(0f, 0f, 45f);
                        clock.GetComponent<Rigidbody2D>().angularVelocity = -4f;
                        GameResume(false);
                    }
                    break;
                case 8:
                    if (!light.GetComponent<LightRotate>().isDay)
                    {
                        uiText.text = tutorialText[textCount];
                        player.GetComponentInChildren<Player_Level>().SendMessage("ExpUp", 100, SendMessageOptions.DontRequireReceiver);
                        clock.GetComponent<Rigidbody2D>().angularVelocity = -10f;
                        GameResume(false);
                    }
                    break;
                case 9:
                    if (light.GetComponent<LightRotate>().isDay)
                    {
                        uiText.text = tutorialText[textCount];
                        GameResume(false);
                    }
                    break;
                case 10:
                    uiText.text = tutorialText[textCount];
                    GameResume(false);
                    break;
                case 11:
                    gameManager.SendMessage("PlayerWin", SendMessageOptions.DontRequireReceiver);
                    break;

            }
            yield return new WaitForSeconds(3f);
        }
    }

    
}
