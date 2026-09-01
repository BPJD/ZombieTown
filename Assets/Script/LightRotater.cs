using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRotater : MonoBehaviour
{
    public float angularVelocityInDay = 4f;
    public float angularVelocityInNight = 8f;
    float angularVelocityInBoss = 2f;
    float angularVelocity;
    Rigidbody2D rig;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        angularVelocity = angularVelocityInDay;
        rig.angularVelocity = -angularVelocity;
        angularVelocityInBoss = angularVelocityInNight * 0.5f;
    }

    public void dayChange(bool _isDay, int _dayCount)
    {
        if(_dayCount == 7)
        {
            rig.angularVelocity = -angularVelocityInBoss;
        }
        else
        {
            if (_isDay)
            {
                rig.angularVelocity = -angularVelocityInDay;
            }
            else
            {
                rig.angularVelocity = -angularVelocityInNight;
            }
        }
    }
}
