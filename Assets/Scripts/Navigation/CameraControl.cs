using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Camera))]
public class CameraControl : MonoBehaviour
{
    [SerializeField, Range(0f, 5f)] private float cameraMoveDuration = 1.5f;
    private bool moving = false;
    private Vector3 targetPosition = Vector3.zero;
    private Vector3 previousPosition = Vector3.zero;
    private float timeElapsed = 0f;

    [SerializeField] private Navigator navigator = null;
    public UnityEvent OnArrive { get; private set; } = new UnityEvent();

    void Awake(){
        navigator.Camera = this;
    }

    public void MoveToRoom(Room room){
        if(!moving && room != null){
            targetPosition = new Vector3(room.Position.x, room.Position.y, transform.position.z);
            previousPosition = transform.position;
            moving = true;
            timeElapsed = 0f;
        }
    }

    void LateUpdate()
    {
        if(moving){
            if(timeElapsed < cameraMoveDuration){
                timeElapsed += Time.deltaTime;
                transform.position = Vector3.Lerp(previousPosition, targetPosition, timeElapsed/cameraMoveDuration);
            }else{
                transform.position = targetPosition;
                moving = false;
                timeElapsed = 0f;
                OnArrive.Invoke();
                OnArrive.RemoveAllListeners();
            }
        }
    }
}
