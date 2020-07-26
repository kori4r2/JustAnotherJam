using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public enum DoorPosition{
    North,
    South,
    East,
    West
}

public class Room : ScriptableObject
{
    public const int size = 50;
    private List<Enemy> enemies = new List<Enemy>();
    private GameObject roomObj;

    public RoomObject northObject;
    public RoomObject southObject;
    public RoomObject eastObject;
    public RoomObject westObject;

    private int id = -1;
    public int ID { get => id; set => id = (id == -1)? value : id; }
    public bool Cleared { get; private set; } = false;
    public Vector3 Position { get => roomObj?.transform.position ?? Vector3.zero; }
    private float openingDelay = 1.0f;
    private bool loadedEnemies = false;
    private int killableCount = 0;
    public int KillableCount{
        get => killableCount;
        set{
            killableCount = value;
            if(killableCount <= 0){
                foreach(Enemy e in enemies){
                    e.Destroy();
                }
                Cleared = true;
                OpenDoors();
            }
        }
    }

    public void Spawn(GameObject roomPrefab, Vector3 position, Navigator navigator, RoomObject north, RoomObject south, RoomObject east, RoomObject west){
        roomObj = GameObject.Instantiate(roomPrefab, position, Quaternion.identity);
        roomObj.name = "Room " + ID;
        Vector3? northPos = roomObj.transform.Find("North")?.localPosition ?? null;
        Vector3? southPos = roomObj.transform.Find("South")?.localPosition ?? null;
        Vector3? eastPos = roomObj.transform.Find("East")?.localPosition ?? null;
        Vector3? westPos = roomObj.transform.Find("West")?.localPosition ?? null;
        if(northPos == null) Debug.LogError("north position not set in the room prefab");
        if(southPos == null) Debug.LogError("south position not set in the room prefab");
        if(eastPos == null) Debug.LogError("east position not set in the room prefab");
        if(westPos == null) Debug.LogError("west position not set in the room prefab");

        // A rotação default é para a saida norte, com os objetos "olhando" para baixo
        Quaternion rotation = Quaternion.identity;
        if(north != null && northPos != null){
            northObject = north;
            north.Spawn(roomObj.transform, Position + (northPos ?? Vector3.zero), rotation);
            if(northObject is Door){
                Door door = (northObject as Door);
                door.Navigator = navigator;
                door.Position = DoorPosition.North;
                door.PlayerEntered.AddListener(SpawnEnemies);
                door.MyRoom = this;
            }
        }

        rotation = Quaternion.AngleAxis(180f, Vector3.back);
        if(south != null && southPos != null){
            southObject = south;
            south.Spawn(roomObj.transform, Position + (southPos ?? Vector3.zero), rotation);
            if(southObject is Door){
                Door door = (southObject as Door);
                door.Navigator = navigator;
                door.Position = DoorPosition.South;
                door.PlayerEntered.AddListener(SpawnEnemies);
                door.MyRoom = this;
            }
        }

        rotation = Quaternion.AngleAxis(90f, Vector3.back);
        if(east != null && eastPos != null){
            eastObject = east;
            east.Spawn(roomObj.transform, Position + (eastPos ?? Vector3.zero), rotation);
            if(eastObject is Door){
                Door door = (eastObject as Door);
                door.Navigator = navigator;
                door.Position = DoorPosition.East;
                door.PlayerEntered.AddListener(SpawnEnemies);
                door.MyRoom = this;
            }
        }

        rotation = Quaternion.AngleAxis(-90f, Vector3.back);
        if(west != null && westPos != null){
            westObject = west;
            west.Spawn(roomObj.transform, Position + (westPos ?? Vector3.zero), rotation);
            if(westObject is Door){
                Door door = (westObject as Door);
                door.Navigator = navigator;
                door.Position = DoorPosition.West;
                door.PlayerEntered.AddListener(SpawnEnemies);
                door.MyRoom = this;
            }
        }

        Disable();
    }

    public void SetWallsState(bool value){
        if(roomObj == null) return;

        foreach(BoxCollider2D boxCollider2D in roomObj.GetComponents<BoxCollider2D>()){
            boxCollider2D.enabled = value;
        }
    }

    public void Disable(){
        northObject?.Disable();
        southObject?.Disable();
        eastObject?.Disable();
        westObject?.Disable();
        roomObj.SetActive(false);
    }

    public RoomObject GetRoomObject(DoorPosition pos){
        switch(pos){
            case DoorPosition.North:
                return northObject;
            case DoorPosition.South:
                return southObject;
            case DoorPosition.East:
                return eastObject;
            case DoorPosition.West:
                return westObject;
        }
        return null;
    }

    public Door GetDoor(DoorPosition pos){
        switch(pos){
            case DoorPosition.North:
                if(northObject is Door)
                    return northObject as Door;
                return null;
            case DoorPosition.South:
                if(southObject is Door)
                    return southObject as Door;
                return null;
            case DoorPosition.East:
                if(eastObject is Door)
                    return eastObject as Door;
                return null;
            case DoorPosition.West:
                if(westObject is Door)
                    return westObject as Door;
                return null;
        }
        return null;
    }

    public IEnumerator DelayedOpenDoors(){
        yield return new WaitForSeconds(openingDelay);
        OpenDoors();
    }

    public void OpenDoors(){
        if(northObject != null && (northObject is Door)){
            (northObject as Door).Open();
        }
        if(southObject != null && (southObject is Door)){
            (southObject as Door).Open();
        }
        if(eastObject != null && (eastObject is Door)){
            (eastObject as Door).Open();
        }
        if(westObject != null && (westObject is Door)){
            (westObject as Door).Open();
        }
    }

    public void Prepare(){
        roomObj.SetActive(true);
        northObject?.Enable();
        southObject?.Enable();
        eastObject?.Enable();
        westObject?.Enable();
    }

    public void SpawnEnemies(){
        SetWallsState(true);
        if(!Cleared && !loadedEnemies){
            loadedEnemies = true;
            if(enemies.Count <= 0){
                KillableCount = 0;
            }else{
                foreach(Enemy enemy in enemies){
                    enemy.Spawn(this);
                }
            }
        }else if(Cleared){
            OpenDoors();
        }
    }
}
