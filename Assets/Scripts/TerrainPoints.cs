using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainPoints : MonoBehaviour
{


    void OnDrawGizmosSelected()
    {
        if (gameObject.tag == "Active")
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawSphere(transform.position, 0.2f);
    }
}
