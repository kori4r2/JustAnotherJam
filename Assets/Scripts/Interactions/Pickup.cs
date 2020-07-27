using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] private Equipment equipment;

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            PlayerController pc = col.GetComponent<PlayerController>();

            if(pc != null)
            {
                pc.Equip(equipment);
                Destroy(gameObject);
            }
        }
    }
}
