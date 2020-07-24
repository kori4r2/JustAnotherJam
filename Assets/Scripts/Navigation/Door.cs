using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : RoomObject
{
    public Room targetRoom;
    public Door targetDoor;
    private bool isOpen = false;

    public void Open(){
        isOpen = true;
        // Muda o sprite e o tipo de colisor do objeto correspondente
    }

    public override void Enable(){
        base.Enable();
        // Fecha a porta sempre que entra na sala
    }
}
