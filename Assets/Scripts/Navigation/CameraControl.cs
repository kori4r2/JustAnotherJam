using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Camera))]
public class CameraControl : MonoBehaviour
{
    [SerializeField, Range(0f, 5f)] private float cameraMoveDuration = 1.5f;
    [SerializeField, Range(0f, 50f)] private float xOffset = 0f;
    private bool moving = false;
    private Vector3 targetPosition = Vector3.zero;
    private Vector3 previousPosition = Vector3.zero;
    private float timeElapsed = 0f;

    [SerializeField] private Navigator navigator = null;
    public UnityEvent OnArrive { get; private set; } = new UnityEvent();

    void Awake(){
        navigator.Camera = this;
    }

    public void MoveToRoom(Room room, bool now = false){
        if(!moving && room != null){
            targetPosition = new Vector3(room.Position.x - xOffset, room.Position.y, transform.position.z);
            if(now){
                previousPosition = targetPosition;
                moving = false;
                timeElapsed = 0f;
                transform.position = targetPosition;
                OnArrive.Invoke();
                OnArrive.RemoveAllListeners();
            }else{
                previousPosition = transform.position;
                moving = true;
                timeElapsed = 0f;
            }
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
