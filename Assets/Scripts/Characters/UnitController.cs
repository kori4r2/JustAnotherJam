using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movable), typeof(Rigidbody2D), typeof(Collider2D))]
public abstract class UnitController : MonoBehaviour, IDamageable
{
    protected const int baseDamage = 50;
    protected const int baseHP = 100;
    protected const int baseSpeed = 10;

    public int MaxHP { get; protected set; } = baseHP;
    public int CurHP { get; protected set; } = baseHP;
    public int Damage { get; protected set; } = baseDamage;
    private int speed = baseSpeed;
    public int Speed {
        get => speed;
        protected set{
            speed = value;
            movable.moveSpeed = speed;
        }
    }

    protected Collider2D col;
    protected Movable movable;
    public Vector2 Direction { get => movable.Direction; }
    public bool CanMove { get => movable.CanMove; set => movable.CanMove = value; }
    [SerializeField] protected Armor armor;
    [SerializeField] protected Arms arms;
    [SerializeField] protected Shoes shoes;

    public virtual void Equip(Armor newArmor){
        armor = newArmor;
        Damage = Mathf.FloorToInt(baseDamage * newArmor.DamageModifier);
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

    public virtual void TakeDamage(UnitController attacker){
        CurHP -= attacker.Damage;
        if(CurHP <= 0){
            Die();
        }
    }

    protected virtual void Die(){
        Destroy(gameObject);
    }

    public void Attack(){
        // Causa dando de acordo com a arma
        // A posicão do atacante está em transform.position
        // A direção do ataque está em movable.Direction
    }

    protected void Awake()
    {
        col = GetComponent<Collider2D>();
        movable = GetComponent<Movable>();
    }

    void Start(){
        Equip(armor);
        Equip(arms);
        Equip(shoes);
    }
}
