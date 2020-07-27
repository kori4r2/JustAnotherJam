using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToNearbyPosition : MoveTo
{
    [SerializeField] private float movementCooldown = 5f;
    [SerializeField] private float walkingRange = 10f;
    [SerializeField, Range(0f, 1f)] private float minMoveRangeRelative = 0.3f;
    private float timer;
    private bool walking = false;
    public bool Walking { get=>walking; }
    private Vector2 targetPosition = Vector2.zero;
    private Movable movable;

    void Awake(){
        movable = GetComponent<Movable>();
        Deactivate();
    }

    public override void Activate(){
        ForceStop();
        timer = 1f;
        base.Activate();
    }

    public override void Deactivate(){
        ForceStop();
        base.Deactivate();
    }

    public void ForceStop(){
        walking = false;
        timer = movementCooldown;
        targetPosition = Vector2.zero;
    }

    public override Vector2 Direction {
        get{
            if(timer <= 0f){
                Vector2 position2D = new Vector2(transform.position.x, transform.position.z);
                if(!walking){
                    // calcula a proxima posição
                    targetPosition = position2D + (walkingRange * Random.insideUnitCircle);
                    walking = true;
                    return (targetPosition - position2D).normalized;
                }else{
                    if(Vector2.Distance(position2D, targetPosition) <= ((movable?.moveSpeed ?? 5f) * Time.fixedDeltaTime)){
                        walking = false;
                        timer = movementCooldown;
                    }else{
                        return (targetPosition - position2D).normalized;
                    }
                }
            }
            walking = false;
            return Vector2.zero;
        }
    }

    protected Vector2 GetNearbyPosition(Vector2 position2D){
        Vector2 random = Random.insideUnitCircle;
        if(random.x < 0 && random.x > -minMoveRangeRelative){
            random.x = -minMoveRangeRelative;
        }
        if(random.x >= 0 && random.x < minMoveRangeRelative){
            random.x = minMoveRangeRelative;
        }
        if(random.y < 0 && random.y > -minMoveRangeRelative){
            random.y = -minMoveRangeRelative;
        }
        if(random.y >= 0 && random.y < minMoveRangeRelative){
            random.y = minMoveRangeRelative;
        }

        Vector2 newPosition = position2D + (walkingRange * random);
        return newPosition;
    }

    public void Update(){
        if(timer > 0f){
            timer -= Time.deltaTime;
        }
    }
}
