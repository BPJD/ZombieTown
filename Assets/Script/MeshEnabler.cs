using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshEnabler : MonoBehaviour
{

    void OnTriggerEnter(Collider col)
    {
        MeshRenderer mesh = col.gameObject.GetComponent<MeshRenderer>();
        if(mesh != null)
        {
            mesh.enabled = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        MeshRenderer mesh = col.gameObject.GetComponent<MeshRenderer>();
        if (mesh != null)
        {
            mesh.enabled = false;
        }
    }
}
