using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private Transform entrance = null;
    [SerializeField] private Transform exit = null;
    public DoorPosition position;
    private Vector2 ExitDirection{
        get{
            switch(position){
                case DoorPosition.North:
                    return Vector2.up;
                case DoorPosition.South:
                    return Vector2.down;
                case DoorPosition.East:
                    return Vector2.right;
                case DoorPosition.West:
                    return Vector2.left;
            }
            return Vector2.zero;
        }
    }

    public bool isOpen = false;
    private bool exiting = false;
    private bool entering = false;
    private UnityEvent PlayerExited = null;
    private UnityEvent PlayerEntered = null;

    public void SetCallbacks(UnityEvent enterEvent, UnityEvent exitEvent){
        if(PlayerEntered == null) PlayerEntered = enterEvent;
        if(PlayerExited == null) PlayerExited = exitEvent;
    }

    public void EnterDoor(PlayerController player){
        entering = true;
        exiting = false;
        player.transform.position = exit.position;
        player.GetComponent<Rigidbody2D>().velocity = -ExitDirection * player.Speed * 0.75f;
    }

    void OnTriggerEnter2D(Collider2D other){
        if(isOpen && !entering && !exiting && other.gameObject.tag == "Player"){
            PlayerController player = other.GetComponent<PlayerController>();
            if(Vector2.Angle(ExitDirection, player.Direction) < 15f){
                // Começa a sair
                player.CanMove = false;
                player.transform.position = entrance.position;
                player.GetComponent<Rigidbody2D>().velocity = ExitDirection * player.Speed * 0.75f;
                PlayerExited?.Invoke();
            }
        }
    }

    void OnTriggerStay2D(Collider2D other){
        if(isOpen && !entering && !exiting && other.gameObject.tag == "Player"){
            PlayerController player = other.GetComponent<PlayerController>();
            if(Vector2.Angle(ExitDirection, player.Direction) < 15f){
                // Começa a sair
                player.CanMove = false;
                player.transform.position = entrance.position;
                player.GetComponent<Rigidbody2D>().velocity = ExitDirection * player.Speed * 0.75f;
                PlayerExited?.Invoke();
                exiting = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other){
        // Quando o jogador acabar de entrar, libera o movimento e chama o callback
        if(!exiting && entering && other.gameObject.tag == "Player"){
            PlayerController player = other.GetComponent<PlayerController>();
            PlayerEntered?.Invoke();
            player.CanMove = true;
            entering = false;
            exiting = false;
        }
    }
}
