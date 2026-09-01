using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satelite_Bullet : MonoBehaviour
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
            tr.Translate(Vector3.forward * 150f * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (!isCollided && (col.CompareTag("Terrain") || col.CompareTag("Building") || col.CompareTag("PlayerBuilding")))
        {
            isCollided = true;
            particle.Play();
            Destroy(gameObject, 1.5f);
        }
        else if(!isCollided && (col.CompareTag("Enemy") || col.CompareTag("EnemyBoss")))
        {
            GetComponent<AudioSource>().Play();
            isCollided = true;
            particle.Play();
            Destroy(gameObject, 1.5f);
            col.gameObject.SendMessage("SetAttacker", player, SendMessageOptions.DontRequireReceiver);
            col.gameObject.SendMessage(damageStr, damage, SendMessageOptions.DontRequireReceiver);
        }
    }

    void SetDamage(int _atk)
    {
        damage = _atk;
    }

}
