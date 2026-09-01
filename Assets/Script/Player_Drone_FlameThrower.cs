using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class Player_Drone_FlameThrower : MonoBehaviour
{
    Vector3 triggerBox = new Vector3(2f, 2f, 10f);
    Player_Drone_Bomb bomb;

    ParticleSystem particle;
    public Transform flamePos;
    public AudioSource audioSource;

    float stopDelay = 0.8f;

    bool isShooting = false;
    bool isSoundPlaying = false;

    string damageStr = "Damaged";

    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<ParticleSystem>();
        bomb = GetComponentInParent<Player_Drone_Bomb>();
        StartCoroutine(FireAttack());
    }

    IEnumerator FireAttack()
    {
        while (true)
        {
            if (bomb.isLockOn)
            {
                isShooting = true;
                stopDelay = 0.8f;
                if (!isSoundPlaying)
                {
                    audioSource.Play();
                    isSoundPlaying = true;
                }
                
            }
            else
            {
                stopDelay -= 0.2f;
                if (stopDelay <= 0)
                {
                    particle.Stop();
                    isShooting = false;
                    audioSource.Stop();
                    isSoundPlaying = false;
                }
            }

            if (isShooting)
            {
                Collider[] touchedObjects = Physics.OverlapBox(flamePos.position, triggerBox * 0.5f, flamePos.rotation);
                
                foreach (Collider touchedObject in touchedObjects)
                {
                    if (touchedObject.CompareTag("Enemy") || touchedObject.CompareTag("EnemyBoss"))
                    {
                        touchedObject.SendMessage("SetAttacker", bomb.player, SendMessageOptions.DontRequireReceiver);
                        touchedObject.SendMessage(damageStr, (int)(bomb.damage * 0.2f), SendMessageOptions.DontRequireReceiver);
                    }
                    //Debug.Log(touchedObjects.Length + " " + bomb.damage);
                }
                particle.Play();
            }

            yield return new WaitForSeconds(bomb.reloadTimes[1]);
        }
    }

}
