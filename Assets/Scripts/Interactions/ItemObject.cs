using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private GameObject pickupItem = null;
    [SerializeField] private GameObject obstacle = null;

    public void Enable(){
        if(pickupItem != null){
            pickupItem.SetActive(true);
        }
        if(obstacle != null){
            obstacle.SetActive(true);
        }
    }

    public void Disable(){
        if(pickupItem != null){
            pickupItem.SetActive(false);
        }
        if(obstacle != null){
            obstacle.SetActive(false);
        }
    }
}
