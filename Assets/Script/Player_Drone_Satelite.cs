using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Drone_Satelite : MonoBehaviour
{
    Transform tr;
    GameObject target;
    Vector3 targetPos;
    List<GameObject> targets = new List<GameObject>();

    Unit_Status playerStat;
    AudioSource audioSource;
    public AudioClip[] gunClips;

    public int weaponType = 0;
    public GameObject[] bullets;
    public int[] magSize = { 1, 12 };
    public float[] reloadTimes = {2f, 5f};
    public float[] reloadTimesDefault = { 0f, 0f };
    public float[] accuracy = { 6f, 0.5f };
    public float[] accuracyDefault = { 0f, 0f };

    int shootCount = 0;
    float reloadTimeCur = 0f;
    
    public float range = 25f;
    public float atkRevision = 1f;
    public float accuracyRevision = 1f;
    public float reloadTimeRevision = 1f;

    bool isReloaded = true;
    bool isLockOn = false;

    WaitForSeconds checkDelay = new WaitForSeconds(0.2f);
    WaitForSeconds delay = new WaitForSeconds(0.1f);
    string atkSet = "SetDamage";

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerStat = GetComponentInParent<Unit_Status>();
        tr = GetComponent<Transform>();
        StartCoroutine(TargetCheck());
        StartCoroutine(TargetShoot());
        accuracyDefault[0] = accuracy[0];
        accuracyDefault[1] = accuracy[1];
        reloadTimesDefault[0] = reloadTimes[0];
        reloadTimesDefault[1] = reloadTimes[1];
    }

    IEnumerator TargetCheck()
    {
        while (true)
        {
            if(target == null)
            {

                Collider[] touchedObjects = Physics.OverlapSphere(new Vector3(tr.position.x, 0f, tr.position.z), range);

                foreach (Collider touchedObject in touchedObjects)
                {
                    if (touchedObject.CompareTag("EnemyBoss"))
                    {
                        target = touchedObject.gameObject;
                        targets.Clear();
                        isLockOn = true;
                        targetPos = target.transform.position;
                    }
                    else if (touchedObject.CompareTag("Enemy"))
                    {
                        targets.Add(touchedObject.gameObject);
                    }
                }

                if(target == null && targets.Count != 0)
                {
                    target = targets[Random.Range(0, targets.Count)];
                    targets.Clear();
                    isLockOn = true;
                    targetPos = target.transform.position;
                }

            }

            else
            {
                if (target.GetComponent<Unit_Status>().player_state == Unit_Status.State.Dead)
                {
                    isLockOn = false;
                    target = null;
                }
                else
                {
                    isLockOn = true;
                    targetPos = target.transform.position;
                }
            }
            yield return checkDelay;
        }
    }

    IEnumerator TargetShoot()
    {
        while (true)
        {
            if (isLockOn && isReloaded)
            {
                if (weaponType == 0)
                {
                    WeaponShoot();
                }
                else
                {
                    WeaponShoot();
                    WeaponShoot();
                }
                audioSource.PlayOneShot(gunClips[weaponType]);
            }
            else if (!isReloaded)
            {
                reloadTimeCur -= 0.1f;
                if(reloadTimeCur <= 0f)
                {
                    isReloaded = true;
                    shootCount = 0;
                }
            }
            yield return delay;
        }
    }

    void WeaponShoot()
    {
        float destX = Random.Range(-accuracy[weaponType], accuracy[weaponType]);
        float destZ = Random.Range(-accuracy[weaponType], accuracy[weaponType]);
        GameObject _bullet = Instantiate(bullets[weaponType], tr.position + new Vector3(Random.Range(-2,2), 0f, Random.Range(-2, 2)), Quaternion.identity);
        _bullet.transform.LookAt(targetPos + new Vector3(destX, 0f, destZ));
        _bullet.SendMessage(atkSet, (int)(playerStat.atk * atkRevision), SendMessageOptions.DontRequireReceiver);

        shootCount++;

        if(shootCount >= magSize[weaponType])
        {
            isReloaded = false;
            reloadTimeCur = reloadTimes[weaponType];
        }

    }

}
