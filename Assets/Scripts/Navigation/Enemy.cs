using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemy")]
public class Enemy : ScriptableObject
{
    [SerializeField] private GameObject prefab = null;
    public Vector2 spawnPosition;
    public bool Alive { get; private set; }
    private EnemyController controller;
    private Room myRoom;

    // Função chamada quando entra na sala
    public void Spawn(Room room){
        myRoom = room;
        Vector3 pos = room.Position + new Vector3(spawnPosition.x, spawnPosition.y);
        controller = GameObject.Instantiate(prefab, pos, Quaternion.identity).GetComponent<EnemyController>();
        controller.so = this;
        if(controller.Killable){
            room.KillableCount++;
        }
    }

    // Função chamada quando a sala é completada (para hazards)
    public void Destroy(){
        if(Alive && controller){
            Destroy(controller.gameObject);
            Alive = false;
            controller = null;
        }
    }

    // Função chamada quando o inimigo morre
    public void Kill(){
        Alive = false;
        controller = null;
        myRoom.KillableCount--;
    }
}
