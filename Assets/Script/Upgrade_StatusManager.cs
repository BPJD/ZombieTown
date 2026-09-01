using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade_StatusManager : MonoBehaviour
{
    public float timeDecrease = 1f; //소요시간 감소량
    public float reqPartDecrease = 1f; //필요자원 감소량
    public float ammoIncrease = 1f; //탄약 획득량

    public int[] enhanceCount = { 0, 0, 0 };
    public Button[] enhanceButtons;
    public Text[] enhanceTexts;
    int upgradeMax = 20;

    public void EnhanceCounter(string _resource)
    {
        switch (_resource)
        {
            case "Food":
                enhanceCount[0]++;
                enhanceTexts[0].text =enhanceCount[0] + " / " + upgradeMax;
                ButtonDeactive(0);
                break;
            case "Part":
                enhanceCount[1]++;
                enhanceTexts[1].text = enhanceCount[1] + " / " + upgradeMax;
                ButtonDeactive(1);
                break;
            case "Ammo":
                enhanceCount[2]++;
                enhanceTexts[2].text = enhanceCount[2] + " / " + upgradeMax;
                ButtonDeactive(2);
                break;
        }
    }
    public void ButtonDeactive(int _resource)
    {
        if (enhanceCount[_resource] == upgradeMax)
        {
            enhanceButtons[_resource].enabled = false;
            enhanceTexts[_resource].color = Color.red;
        }
    }
}