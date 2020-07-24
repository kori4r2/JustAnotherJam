using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movable), typeof(Rigidbody2D))]
public abstract class UnitController : MonoBehaviour, IDamageable
{
    protected const int baseAttack = 50;
    protected const int baseHP = 100;
    protected const int baseSpeed = 10;

    public int MaxHP { get; protected set; } = baseHP;
    public int CurHP { get; protected set; } = baseHP;
    public int Attack { get; protected set; } = baseAttack;
    private int speed = baseSpeed;
    public int Speed {
        get => speed;
        protected set{
            speed = value;
            movable.moveSpeed = speed;
        }
    }

    protected Movable movable;
    [SerializeField] protected Armor armor;
    [SerializeField] protected Arms arms;
    [SerializeField] protected Shoes shoes;

    public virtual void Equip(Armor newArmor){
        Attack = Mathf.FloorToInt(baseAttack * newArmor.AttackModifier);
        Speed = Mathf.FloorToInt(baseSpeed * newArmor.SpeedModifier);
        float hpPercentage = MaxHP / (1.0f * CurHP);
        MaxHP = Mathf.FloorToInt(baseHP * newArmor.HealthModifier);
        CurHP = Mathf.Max(Mathf.FloorToInt(hpPercentage * MaxHP), 1);
        // Muda o sprite aqui
    }

    public virtual void Equip(Arms newArms){
        arms = newArms;
        // Muda o sprite aqui
    }

    public virtual void Equip(Shoes newShoes){
        shoes = newShoes;
        // Muda o sprite aqui
    }

    public virtual void Damage(UnitController attacker){
        CurHP -= attacker.Attack;
        if(CurHP <= 0){
            Die();
        }
    }

    protected virtual void Die(){
        Destroy(gameObject);
    }

    void Awake()
    {
        movable = GetComponent<Movable>();
    }

    void Start(){
        Equip(armor);
        Equip(arms);
        Equip(shoes);
    }
}
