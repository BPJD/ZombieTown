using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_FloatingText : MonoBehaviour
{
    public Text[] resTexts;
    public Image[] resIcons;
    public GameObject[] texts;
    // Start is called before the first frame update
    
    Transform tr;
    Color textCol;
    Color iconCol;


    void Start()
    {
        Destroy(gameObject, 5f);
        tr = GetComponent<Transform>();
        textCol = resTexts[0].color;
        iconCol = resIcons[0].color;
    }

    // Update is called once per frame
    void Update()
    {
        tr.Translate(Vector3.up * 30f * Time.deltaTime);

        if (textCol.a > 0)
        {
            textCol.a -= 0.4f * Time.deltaTime;
            iconCol.a = textCol.a;
            resTexts[0].color = textCol;
            resTexts[1].color = textCol;
            resTexts[2].color = textCol;
            resIcons[0].color = iconCol;
            resIcons[1].color = iconCol;
            resIcons[2].color = iconCol;
        }
    }

    void SetRes(int[] res)
    {
        resTexts[0].text = "+ " + res[0].ToString();
        resTexts[1].text = "+ " + res[1].ToString();
        resTexts[2].text = "+ " + res[2].ToString();
        //StartCoroutine(TextMake());
    }

    IEnumerator TextMake()
    {
        texts[0].SetActive(true);
        yield return new WaitForSeconds(0.8f);
        texts[1].SetActive(true);
        yield return new WaitForSeconds(0.8f);
        texts[2].SetActive(true);
    }
}
