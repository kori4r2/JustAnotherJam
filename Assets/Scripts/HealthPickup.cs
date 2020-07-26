using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [Range(0,1)]
    [Tooltip("In percent of total health")]
    public float cureAmount = 0.2f; 
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            PlayerController pc = col.GetComponent<PlayerController>();

            if(pc != null)
            {
                pc.CurePlayer(cureAmount);
            }
        }

    }

}
