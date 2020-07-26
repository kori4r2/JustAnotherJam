using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    private class RoomInfo{
        public Room SO { get; private set; } = (Room)ScriptableObject.CreateInstance(typeof(Room));
        public Dictionary<DoorPosition, RoomObject> objects = new Dictionary<DoorPosition, RoomObject>();
        public Vector2 coord = Vector2.zero;
        private bool spawned = false;
        public RoomInfo(){
            objects.Add(DoorPosition.North, null);
            objects.Add(DoorPosition.South, null);
            objects.Add(DoorPosition.East, null);
            objects.Add(DoorPosition.West, null);
        }

        public void Spawn(GameObject prefab, Navigator navigator){
            if(!spawned){
                SO.Spawn(prefab, GetWorldPos(), navigator, objects[DoorPosition.North], objects[DoorPosition.South], objects[DoorPosition.East], objects[DoorPosition.West]);
                navigator.AddRoom(SO);
                spawned = true;
            }
        }

        public Vector3 GetWorldPos(){
            return new Vector3(coord.x * Room.size, coord.y * Room.size);
        }
    }

    [SerializeField] private int nRooms = 20;
    [Header("Scene References")]
    [SerializeField] private Navigator navigator = null;
    [SerializeField] private PlayerController player = null;
    [Header("Prefabs")]
    [SerializeField] private GameObject firstRoom = null;
    [SerializeField] private GameObject lastRoom = null;
    [SerializeField] private List<GameObject> roomPrefabs = new List<GameObject>();
    [SerializeField] private Door door = null;
    [SerializeField] private List<Enemy> enemies = new List<Enemy>();
    [SerializeField] private RoomObject slimeItem = null;
    [SerializeField] private RoomObject humanItem = null;
    [SerializeField] private RoomObject elfItem = null;
    [SerializeField] private RoomObject orcItem = null;
    // Start is called before the first frame update
    void Start()
    {
        navigator.Restart();
        GenerateMap();
    }

    private Vector2 NextDirection(Vector2 curDir, Vector2 curPos, Dictionary<Vector2, RoomInfo> map){
        Vector2 nextDir = curDir;

        // rotaciona o vetor 3 vezes pra pegar a direção da direita
        for(int i = 0; i < 3; i++) nextDir = Vector2.Perpendicular(nextDir);
        // Escolhe uma direção aleatoria entre direita, frente e esquerda
        for(int i = 0; i < Random.Range(0, 3); i++) nextDir = Vector2.Perpendicular(nextDir);
        // Se certifica que a direção é valida, checando as outras possíveis caso não seja
        for(int i = 0; i < 4 && ((map[curPos].objects[GetExitPos(nextDir)] != null) || map.ContainsKey(curPos + nextDir)); i++){
            nextDir = Vector2.Perpendicular(nextDir);
        }

        // Retorna vetor zero quando não há direção livre
        if(map[curPos].objects[GetExitPos(nextDir)] != null || map.ContainsKey(curPos + nextDir)){
            return Vector2.zero;
        }else{
            return nextDir;
        }
    }

    private DoorPosition GetExitPos(Vector2 nextDirection){
        if(nextDirection == Vector2.up){
            return DoorPosition.North;
        }else if(nextDirection == Vector2.down){
            return DoorPosition.South;
        }else if(nextDirection == Vector2.right){
            return DoorPosition.East;
        }else if(nextDirection == Vector2.left){
            return DoorPosition.West;
        }
        return DoorPosition.North;
    }

    private DoorPosition GetEntrancePos(Vector2 curDirection){
        if(curDirection == Vector2.up){
            return DoorPosition.South;
        }else if(curDirection == Vector2.down){
            return DoorPosition.North;
        }else if(curDirection == Vector2.right){
            return DoorPosition.West;
        }else if(curDirection == Vector2.left){
            return DoorPosition.East;
        }
        return DoorPosition.North;
    }

    private void GenerateMap(){
        if(navigator && player){
            Dictionary<Vector2, RoomInfo> map = new Dictionary<Vector2, RoomInfo>();
            Vector2 curPos = Vector2.zero;
            Vector2 curDirection = Vector2.up;
            List<Vector2> directionHistory = new List<Vector2>();
            int curID = 0;

            // Cria a primeira sala do mapa
            map.Add(curPos, new RoomInfo());
            map[curPos].SO.ID = curID;
            map[curPos].coord = curPos;
            // Adiciona uma porta para cima
            map[curPos].objects[DoorPosition.North] = Instantiate(door);
            Door curDoor = map[curPos].objects[DoorPosition.North] as Door;
            curDoor.targetDoorPosition = DoorPosition.South;
            // Incrementa o id
            curID++;
            curDoor.targetRoomID = curID;
            curPos += curDirection;
            directionHistory.Add(Vector2.down);

            while(curID < nRooms){
                // Cria a sala em questão
                map.Add(curPos, new RoomInfo());
                map[curPos].SO.ID = curID;
                map[curPos].coord = curPos;

                // Cria a porta de entrada
                DoorPosition entrancePos = GetEntrancePos(curDirection);
                map[curPos].objects[entrancePos] = Instantiate(door);
                curDoor = map[curPos].objects[entrancePos] as Door;
                curDoor.targetRoomID = map[curPos-curDirection].SO.ID;
                curDoor.targetDoorPosition = GetExitPos(curDirection);

                // Se for a ultima sala não precisa criar uma saída ou colocar itens
                curID++;
                if(curID < nRooms){
                    // Adiciona os inimigos
                    for(int i = 0; i < 3; i++){
                        Enemy newEnemy = Instantiate(enemies[Random.Range(0, enemies.Count)]);
                        if(newEnemy != null){
                            newEnemy.spawnPosition = new Vector3(-5 + (i * 5), 0, 0);
                            map[curPos].SO.enemies.Add(newEnemy);
                        }
                    }
                    // TO DO calcula a chance e coloca um item em um dos espaços vazios

                    // Seleciona a posição da saída na sala atual
                    Vector2 nextDir = NextDirection(curDirection, curPos, map);
                    while(nextDir == Vector2.zero){
                        // Volta uma sala e recalcula a proxima direção até achar uma saida valida
                        curPos -= curDirection;
                        directionHistory.RemoveAt(directionHistory.Count-1);
                        curDirection = directionHistory[directionHistory.Count-1];
                        nextDir = NextDirection(curDirection, curPos, map);
                    }

                    // Cria a porta de saída
                    DoorPosition exitPos = GetExitPos(nextDir);
                    map[curPos].objects[exitPos] = Instantiate(door);
                    curDoor = map[curPos].objects[exitPos] as Door;
                    curDoor.targetDoorPosition = GetEntrancePos(nextDir);
                    curDoor.targetRoomID = curID;
                    curDirection = nextDir;
                    curPos += curDirection;
                    directionHistory.Add(curDirection);
                }else{
                    // Spawna a ultima sala e a primeira sala
                    map[curPos].Spawn(lastRoom, navigator);
                    map[Vector2.zero].Spawn(firstRoom, navigator);
                }
            }

            foreach(RoomInfo info in map.Values){
                GameObject selectedRoom = roomPrefabs[Random.Range(0, roomPrefabs.Count)];
                info.Spawn(selectedRoom, navigator);
            }

            player.transform.position = map[Vector2.zero].GetWorldPos();
            navigator.Camera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, navigator.Camera.transform.position.z);

            map.Clear();
            navigator.StartNavigation();
        }
    }
}
