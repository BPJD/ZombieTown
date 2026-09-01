using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Drone_Bomb : MonoBehaviour
{
    public GameObject particleObj;
    Transform particleObjTr;
    ParticleSystem particle;
    Transform tr;

    string playerStr = "PlayerSystem";
    string damageStr = "Damaged";
    public GameObject player;
    Unit_Status playerStat;

    GameObject target;
    public int damage = 0;
    float shortDis;
    public bool isLockOn = false;
    public int weaponType = 0;
    public float[] reloadTimes = { 5f, 0.2f };
    public float[] reloadTimesDefault = { 0f, 0f };
    float reloadTimeCur = 0f;
    public float[] range = { 20f, 8f };
    public float damageRevision = 1f;
    public float reloadTimeRevision = 1f;
    public GameObject flameThrower;
    public AudioClip[] gunClips;
    AudioSource audioSource;


    List<GameObject> targets = new List<GameObject>();

    float explodeDelay = 0.1f;

    public float explodeRadius = 3f;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        particle = particleObj.GetComponent<ParticleSystem>();
        particleObjTr = particle.GetComponent<Transform>();
        tr = GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag(playerStr);
        playerStat = GetComponentInParent<Unit_Status>();
        StartCoroutine(TargetCheck());
        StartCoroutine(TargetShoot());
        reloadTimesDefault[0] = reloadTimes[0];
        reloadTimesDefault[1] = reloadTimes[1];
    }

    IEnumerator TargetCheck()
    {
        while (true)
        {
            if (!isLockOn)
            {
                Collider[] touchedObjects = Physics.OverlapSphere(tr.position, range[weaponType]);
                foreach (Collider touchedObject in touchedObjects)
                {
                    if (touchedObject.CompareTag("Enemy") || touchedObject.CompareTag("EnemyBoss"))
                    {
                        targets.Add(touchedObject.gameObject);
                    }
                }

                if (targets.Count != 0)
                {

                    shortDis = Vector3.Distance(gameObject.transform.position, targets[0].transform.position); // 첫번째를 기준으로 잡아주기 

                    target = targets[0]; // 첫번째를 먼저 

                    foreach (GameObject found in targets)
                    {
                        float Distance = Vector3.Distance(tr.position, found.transform.position);

                        if (Distance < shortDis) // 위에서 잡은 기준으로 거리 재기
                        {
                            target = found;
                            shortDis = Distance;
                        }
                    }
                }

                if (target != null)
                {
                    isLockOn = true;
                    targets.Clear();
                }
                else
                {
                    isLockOn = false;
                    targets.Clear();
                }
            }
            else
            {
                if (target.GetComponent<Unit_Status>().player_state == Unit_Status.State.Dead || Vector3.Distance(tr.position, target.transform.position) >= range[weaponType])
                {
                    target = null;
                    isLockOn = false;
                }
            }

            damage = (int)(playerStat.atk * damageRevision);

            yield return new WaitForSeconds(0.8f);
        }
    }

    void Update()
    {
        if (isLockOn)
        {
            tr.LookAt(target.transform.position + (Vector3.up * 2f));
        }
    }


    IEnumerator TargetShoot()
    {
        while (true)
        {
            if (weaponType == 0 && isLockOn && reloadTimeCur <= 0f)
            {
                Explode();
                reloadTimeCur = reloadTimes[0];
                audioSource.PlayOneShot(gunClips[0]);
            }
            else if(weaponType == 1)
            {
                flameThrower.SetActive(true);
                audioSource.clip = gunClips[1];
                audioSource.loop = true;
                StopCoroutine(TargetShoot());
            }
            reloadTimeCur -= explodeDelay;
            yield return new WaitForSeconds(explodeDelay);
        }
    }

    void Explode()
    {
        particleObjTr.position = target.transform.position;
        particle.Play();

        Collider[] touchedObjects = Physics.OverlapSphere(particleObjTr.position, explodeRadius);
        foreach (Collider touchedObject in touchedObjects)
        {
            if (touchedObject.CompareTag("Enemy") || touchedObject.CompareTag("EnemyBoss"))
            {
                touchedObject.gameObject.SendMessage("SetAttacker", player, SendMessageOptions.DontRequireReceiver);
                touchedObject.SendMessage(damageStr, damage, SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    
}
