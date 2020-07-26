﻿using System.Collections;
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
    private List<IDamageable> targetsHit = new List<IDamageable>();
    // Start is called before the first frame update
    void Awake()
    {
        if(sr == null)sr = GetComponent<SpriteRenderer>();
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
