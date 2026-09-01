using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SateliteFloating : MonoBehaviour
{
    Transform tr;
    float spd = 3f;
    public GameObject sateliteFeature;
    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        tr.position = GameObject.FindGameObjectWithTag("Player").transform.position + new Vector3(0f, 2.1f, 2.5f);
    }

    // Update is called once per frame
    void Update()
    {
        tr.Translate(Vector3.up * spd * Time.deltaTime);
        tr.Rotate(Vector3.up * 30f * Time.deltaTime);
        if(tr.position.y >= 40)
        {
            sateliteFeature.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
