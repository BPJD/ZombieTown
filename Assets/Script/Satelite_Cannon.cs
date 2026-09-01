using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satelite_Cannon : MonoBehaviour
{
    ParticleSystem particle;
    Transform tr;
    bool isCollided = false;
    int damage = 0;
    GameObject player;


    string damageStr = "Damaged";

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        particle = GetComponent<ParticleSystem>();
        player = GameObject.FindGameObjectWithTag("PlayerSystem");
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCollided)
        {
            tr.Translate(Vector3.forward * 90f * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (!isCollided && (col.CompareTag("Terrain") || col.CompareTag("Building") || col.CompareTag("PlayerBuilding")))
        {
            Collider[] touchedObjects = Physics.OverlapSphere(transform.position, 5f);
            foreach (Collider touchedObject in touchedObjects)
            {
                if (touchedObject.CompareTag("Enemy") || touchedObject.CompareTag("EnemyBoss"))
                {
                    touchedObject.gameObject.SendMessage("SetAttacker", player, SendMessageOptions.DontRequireReceiver);
                    touchedObject.SendMessage(damageStr, damage, SendMessageOptions.DontRequireReceiver);
                    //Debug.Log("Hit");
                }
            }

            GetComponent<AudioSource>().Play();
            isCollided = true;
            particle.Play();
            Destroy(gameObject, 3f);
        }

    }


    void SetDamage(int _atk)
    {
        damage = (int)(_atk * 5f);
    }

}
