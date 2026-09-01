using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Drone_Rotate : MonoBehaviour
{
    Transform tr;
    public float speed = 30f;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        tr.Rotate(new Vector3(0, 1, 0) * speed * Time.deltaTime);
    }
}
