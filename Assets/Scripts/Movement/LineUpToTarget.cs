using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineUpToTarget : MoveTo
{
    public float idealRange = 0f;
    public Transform target = null;
    private Rigidbody2D rigid2D = null;

    public void Awake(){
        rigid2D = GetComponent<Rigidbody2D>();
        Deactivate();
    }

    override public Vector2 Direction{
        get{
            if(target){
                Vector2 distance = new Vector2(target.position.x, target.position.y) - rigid2D.position;
                Vector2 offsetDirection = Vector2.zero;
                if(Mathf.Abs(distance.y) > Mathf.Abs(distance.x)){
                    if(distance.y > 0){
                        offsetDirection = Vector2.down;
                    }else{
                        offsetDirection = Vector2.up;
                    }
                }else{
                    if(distance.x > 0){
                        offsetDirection = Vector2.left;
                    }else{
                        offsetDirection = Vector2.right;
                    }
                }
                Vector2 targetPosition = new Vector2(target.position.x, target.position.y);
                targetPosition += offsetDirection * idealRange;
                if(distance.magnitude > 10 * float.Epsilon){
                    return (targetPosition - rigid2D.position).normalized;
                }else{
                    return Vector2.zero;
                }
            }
            return Vector2.zero;
        }
    }
}
