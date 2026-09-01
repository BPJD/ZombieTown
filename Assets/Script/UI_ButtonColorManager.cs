using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ButtonColorManager : MonoBehaviour
{
    public Image[] weaponButtons;

    // Start is called before the first frame update
    void Start()
    {
        weaponButtons = GetComponentsInChildren<Image>();
    }

    public void WeaponButtonColorSet(string _button)
    {
        weaponButtons[0].sprite = Resources.Load<Sprite>("UI_Btn/btn_idle") as Sprite;
        weaponButtons[2].sprite = Resources.Load<Sprite>("UI_Btn/btn_idle") as Sprite;
        weaponButtons[4].sprite = Resources.Load<Sprite>("UI_Btn/btn_idle") as Sprite;
        weaponButtons[6].sprite = Resources.Load<Sprite>("UI_Btn/btn_idle") as Sprite;
        weaponButtons[8].sprite = Resources.Load<Sprite>("UI_Btn/btn_idle") as Sprite;
        switch (_button)
        {
            case "Pistol":
                weaponButtons[0].sprite = Resources.Load<Sprite>("UI_Btn/btn_selected") as Sprite;
                break;
            case "Revolver":
                weaponButtons[2].sprite = Resources.Load<Sprite>("UI_Btn/btn_selected") as Sprite;
                break;
            case "SMG":
                weaponButtons[4].sprite = Resources.Load<Sprite>("UI_Btn/btn_selected") as Sprite;
                break;
            case "SniperRifle":
                weaponButtons[6].sprite = Resources.Load<Sprite>("UI_Btn/btn_selected") as Sprite;
                break;
            case "AutoRifle":
                weaponButtons[8].sprite = Resources.Load<Sprite>("UI_Btn/btn_selected") as Sprite;
                break;
        }

    }
}
