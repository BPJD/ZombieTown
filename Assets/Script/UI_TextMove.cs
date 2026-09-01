using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TextMove : MonoBehaviour
{
    Transform tr;
    Text thisText;
    Color col;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 8f);
        tr = GetComponent<Transform>();
        col = thisText.color;
    }

    // Update is called once per frame
    void Update()
    {
        tr.Translate(Vector3.up * 20f * Time.deltaTime);

        if (col.a > 0)
        {
            col.a -= 0.4f * Time.deltaTime;
            thisText.color = col;
        }
    }

    void TextSet(string _text)
    {
        thisText = GetComponent<Text>();
        thisText.text = _text;
    }
}
