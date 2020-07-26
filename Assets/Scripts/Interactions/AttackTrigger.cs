﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CompositeCollider2D), typeof(SpriteRenderer))]
public class AttackTrigger : MonoBehaviour
{
    private SpriteRenderer sr = null;
    private List<Collider2D> colliders = new List<Collider2D>();
    private UnitController unit = null;
    private bool attacking = false;
    private List<IDamageable> targetsHit = new List<IDamageable>();
    // Start is called before the first frame update
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;
        colliders.AddRange(GetComponents<Collider2D>());
        colliders.Remove(GetComponent<CompositeCollider2D>());
        SetColliderState(false);
        attacking = false;
    }

    public void SetColliderState(bool state){
        foreach(Collider2D col in colliders){
            col.enabled = state;
        }
    }

    public void PrepareAttack(UnitController unit){
        attacking = true;
        sr.enabled = true;
        this.unit = unit;
        UpdateRotation(unit.Direction);
        targetsHit.Clear();
    }

    public void Attack(){
        SetColliderState(true);
    }

    public void EndAttack(){
        attacking = false;
        SetColliderState(false);
        sr.enabled = false;
        this.unit = null;
        targetsHit.Clear();
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

    // Update is called once per frame
    void Update()
    {
        if(attacking){
            UpdateRotation(unit.Direction);
        }
    }

    void OntriggerEnter2D(Collider2D other){
        if(other.gameObject.tag != unit.gameObject.tag){
            IDamageable target = other.GetComponent<IDamageable>();
            if(target != null && !targetsHit.Contains(target)){
                targetsHit.Add(target);
                target.TakeDamage(unit);
            }
        }
    }
}
