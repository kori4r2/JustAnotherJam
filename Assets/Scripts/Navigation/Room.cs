using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : ScriptableObject
{
    private List<Enemy> enemies;
    private GameObject roomObj;

    public RoomObject northObject;
    public RoomObject southObject;
    public RoomObject eastObject;
    public RoomObject westObject;

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

    public void Spawn(GameObject roomPrefab, Vector3 position, RoomObject north, RoomObject south, RoomObject east, RoomObject west){
        roomObj = GameObject.Instantiate(roomPrefab, position, Quaternion.identity);
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
        }

        rotation = Quaternion.AngleAxis(180f, Vector3.back);
        if(south != null && southPos != null){
            southObject = south;
            south.Spawn(roomObj.transform, Position + (southPos ?? Vector3.zero), rotation);
        }

        rotation = Quaternion.AngleAxis(-90f, Vector3.back);
        if(east != null && eastPos != null){
            eastObject = east;
            east.Spawn(roomObj.transform, Position + (eastPos ?? Vector3.zero), rotation);
        }

        rotation = Quaternion.AngleAxis(90f, Vector3.back);
        if(west != null && westPos != null){
            westObject = west;
            west.Spawn(roomObj.transform, Position + (westPos ?? Vector3.zero), rotation);
        }

        Disable();
    }

    public void Disable(){
        northObject?.Disable();
        southObject?.Disable();
        eastObject?.Disable();
        westObject?.Disable();
        roomObj.SetActive(false);
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
        if(!Cleared && !loadedEnemies){
            loadedEnemies = true;
            foreach(Enemy enemy in enemies){
                enemy.Spawn(this);
            }
        }else if(Cleared){
            OpenDoors();
        }
    }
}
