using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_UI_OnScreen : MonoBehaviour
{
    public Building_UI ui;

    void Update()
    {
        transform.position = Camera.main.WorldToScreenPoint(ui.selectedBuilding.transform.position + (Vector3.up * 8f)) + new Vector3(0f, 30f, 0f);
    }
}
