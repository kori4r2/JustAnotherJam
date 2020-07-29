﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CompositeCollider2D))]
public class AttackTrigger : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer sr = null;
    private List<Collider2D> colliders = new List<Collider2D>();
    private CompositeCollider2D composite = null;
    ContactFilter2D filter = new ContactFilter2D();
    private UnitController unit = null;
    public UnitController Unit { get => unit; set => unit = (unit == null)? value : unit; }
    private bool attacking = false;
    // Start is called before the first frame update
    void Awake()
    {
        if(sr == null) sr = GetComponent<SpriteRenderer>();
        if(sr != null){
            sr.enabled = false;
        }
        colliders.AddRange(GetComponents<Collider2D>());
        composite = GetComponent<CompositeCollider2D>();
        colliders.Remove(composite);

        filter = new ContactFilter2D();
        filter.NoFilter();

        SetColliderState(true);
        attacking = false;
    }

    public void SetColliderState(bool state){
        foreach(Collider2D col in colliders){
            col.isTrigger = true;
            col.enabled = state;
        }
    }

    public void PrepareAttack(){
        if(sr != null){
            sr.enabled = true;
        }
        if(unit != null){
            attacking = true;
        }
        UpdateRotation(unit.Direction);
    }

    public void Attack(){
        if(unit != null && attacking){
            Collider2D[] hits = new Collider2D[10];
            composite.OverlapCollider(filter, hits);
            foreach(Collider2D hit in hits){
                if(hit == null) break;
                if(hit != null && hit.gameObject.tag != unit.gameObject.tag){
                    IDamageable target = hit.GetComponent<IDamageable>();
                    if(target != null){
                        target.TakeDamage(unit);
                    }
                }
            }
        }
    }

    public void EndAttack(){
        attacking = false;
        if(sr != null){
            sr.enabled = false;
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
