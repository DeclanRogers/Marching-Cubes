using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainPoints : MonoBehaviour
{

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
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
