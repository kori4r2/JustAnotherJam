using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CompositeCollider2D))]
public class AttackTrigger : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer sr = null;
    private List<Collider2D> colliders = new List<Collider2D>();
    private UnitController unit = null;
    private bool attacking = false;
    private List<IDamageable> targets = new List<IDamageable>();
    // Start is called before the first frame update
    void Awake()
    {
        if(sr == null) sr = GetComponent<SpriteRenderer>();
        if(sr != null){
            sr.enabled = false;
        }
        colliders.AddRange(GetComponents<Collider2D>());
        colliders.Remove(GetComponent<CompositeCollider2D>());
        SetColliderState(true);
        attacking = false;
    }

    public void SetColliderState(bool state){
        foreach(Collider2D col in colliders){
            col.isTrigger = true;
            col.enabled = state;
        }
    }

    public void PrepareAttack(UnitController thisUnit){
        attacking = true;
        if(sr != null){
            sr.enabled = true;
        }
        unit = thisUnit;
        UpdateRotation(unit.Direction);
    }

    public void Attack(){
        if(unit != null && attacking){
            foreach(IDamageable target in targets){
                if(target != null){
                    target.TakeDamage(unit);
                }
            }
            unit = null;
        }
    }

    public void EndAttack(){
        attacking = false;
        if(sr != null){
            sr.enabled = false;
        }
        this.unit = null;
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag != unit.gameObject.tag){
            IDamageable target = other.GetComponent<IDamageable>();
            if(target != null && !targets.Contains(target)){
                targets.Add(target);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.tag != unit.gameObject.tag){
            IDamageable target = other.GetComponent<IDamageable>();
            if(target != null && !targets.Contains(target)){
                targets.Remove(target);
            }
        }
    }

    private void UpdateRotation(Vector2 direction){
        if(direction == Vector2.down){
            transform.rotation = Quaternion.identity;
        }else if(direction == Vector2.up){
            transform.rotation = Quaternion.AngleAxis(180f, Vector3.back);
        }else if(direction == Vector2.right){
            transform.rotation = Quaternion.AngleAxis(-90f, Vector3.back);
        }else if(direction == Vector2.left){
            transform.rotation = Quaternion.AngleAxis(90f, Vector3.back);
        }
    }
}
