using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Collider2D))]
public class Trap : MonoBehaviour
{
    private Animator anim = null;
    private PlayerController player = null;

    public void Awake(){
        anim = GetComponent<Animator>();
    }

    public void KillPlayer(){
        player.ForceDie(true);
    }

    public void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Player"){
            player = other.GetComponent<PlayerController>();
            player.invulnerable = true;
            player.CanMove = false;
            player.transform.position = new Vector3(transform.position.x, transform.position.y, player.transform.position.z);
            anim.SetTrigger("Activate");
        }
    }
}
