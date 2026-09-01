using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_UI : MonoBehaviour
{
    public GameObject mainUI, optionUI, creditUI;

    public void StartClicked()
    {
        SceneManager.LoadScene(1);
    }

    public void TutorialClicked()
    {
        SceneManager.LoadScene(2);
    }

    public void OptionClicked()
    {
        optionUI.SetActive(true);
        mainUI.SetActive(false);
    }

    public void ExitClicked()
    {
        Application.Quit();
    }

    public void CreditClicked()
    {
        creditUI.SetActive(true);
        mainUI.SetActive(false);
    }

    public void BackToMainClicked()
    {
        creditUI.SetActive(false);
        optionUI.SetActive(false);
        mainUI.SetActive(true);
    }

}
