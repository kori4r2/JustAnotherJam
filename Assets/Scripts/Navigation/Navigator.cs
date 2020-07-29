using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[CreateAssetMenu(fileName="Navigator", menuName="Navigator")]
public class Navigator : ScriptableObject
{
    // Não deve persistir entre mudanças de cena, so pode ser alterado uma vez
    private CameraControl camera = null;
    public CameraControl Camera { get => camera; set => camera = (camera == null)? value : camera; }
    private PlayerController player = null;
    public PlayerController Player { get => player; set => player = (player == null)? value : player; }
    [SerializeField] private bool navigating = false;

    private Dictionary<int, Room> rooms = new Dictionary<int, Room>();
    private ReadOnlyDictionary<int, Room> roomsReadOnly = null;
    public ReadOnlyDictionary<int, Room> Rooms{
        get{
            if(roomsReadOnly == null){
                roomsReadOnly = new ReadOnlyDictionary<int, Room>(rooms);
            }
            return roomsReadOnly;
        }
    } 

    public void Restart(){
        rooms = new Dictionary<int, Room>();
        roomsReadOnly = null;
        navigating = false;
    }

    void Awake(){
        Restart();
    }

    public void StartNavigation(){
        if(player != null && camera != null){
            rooms[0].Prepare();
            rooms[0].OpenDoors();
            navigating = true;
            // Começa a "cutscene" na primeira sala?
            TimeTracking.instance?.OnNavigatorReady(this);
        }
    }

    // É preciso garantir no gerador de níveis que o id é unico para cada sala
    public void AddRoom(Room newRoom){
        if(!navigating){
            rooms.Add(newRoom.ID, newRoom);
        }
    }
}
