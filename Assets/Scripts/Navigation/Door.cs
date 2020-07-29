using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Door", menuName = "Door")]
public class Door : RoomObject
{
    public int targetRoomID;
    public DoorPosition targetDoorPosition;
    private Navigator navigator = null;
    public Navigator Navigator { set => navigator = (navigator == null)? value : null; }
    private Room myRoom = null;
    public Room MyRoom{
        get => myRoom;
        set => myRoom = (myRoom == null)? value : myRoom;
    }
    private bool isOpen = false;
    public UnityEvent PlayerExited { get; private set; } = new UnityEvent();
    public UnityEvent PlayerEntered { get; private set; } = new UnityEvent();
    private DoorTrigger trigger;
    private DoorPosition position;
    public DoorPosition Position{
        get => position;
        set{
            if(trigger){
                trigger.position = value;
            }
            position = value;
        }
    }

    public override void Spawn(Transform parent, Vector3 pos, Quaternion rotation){
        base.Spawn(parent, pos, rotation);
        isOpen = false;
        trigger = obj.GetComponent<DoorTrigger>();
        trigger.SetCallbacks(PlayerEntered, PlayerExited);
        PlayerExited?.AddListener(MoveCameraToTarget);
    }

    public void MoveCameraToTarget(){
        myRoom.SetWallsState(false);
        navigator.Rooms[targetRoomID].Prepare();
        navigator.Camera.MoveToRoom(navigator.Rooms[targetRoomID]);
        navigator.Camera.OnArrive?.AddListener(EnterNextRoom);
    }

    public void EnterNextRoom(){
        myRoom.Disable();
        navigator.Rooms[targetRoomID].SetWallsState(false);
        navigator.Rooms[targetRoomID].GetDoor(targetDoorPosition).trigger.EnterDoor(navigator.Player);
        myRoom.SetWallsState(true);
    }

    public void Open(){
        isOpen = true;
        trigger.IsOpen = true;
        // Muda o sprite e o tipo de colisor do objeto correspondente
    }

    public override void Enable(){
        base.Enable();
        // Fecha a porta sempre que entra na sala
        trigger.IsOpen = false;
        Debug.Log("Enable " + this.name);
    }
}
