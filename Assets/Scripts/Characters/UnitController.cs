using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movable), typeof(Rigidbody2D), typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
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
    protected Animator anim;
    public Vector2 Direction { get => movable.Direction; }
    public bool CanMove { get => movable.CanMove; set => movable.CanMove = value; }
    [SerializeField, Range(0f, 5f)] protected float invulnerableTime = 1f;
    [SerializeField] protected Armor armor;
    [SerializeField] protected Arms arms;
    [SerializeField] protected Shoes shoes;
    private AttackTrigger atkTrigger = null;
    private float timer = 0f;
    private bool invulnerable = false;

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
        if(atkTrigger){
            Destroy(atkTrigger.gameObject);
            atkTrigger = arms.AddTrigger(transform);
        }
    }

    public virtual void Equip(Shoes newShoes){
        shoes = newShoes;
        // Muda o sprite aqui
    }

    public virtual void TakeDamage(UnitController attacker){
        CurHP -= attacker.Damage;
        if(CurHP <= 0){
            Die();
        }else{
            timer = invulnerableTime;
            invulnerable = true;
        }
    }

    protected virtual void Die(){
        Destroy(gameObject);
    }

    protected void StartAttack(){
        CanMove = false;
        atkTrigger?.PrepareAttack(this);
        // TO DO: ativar o trigger de animação
    }

    // Função que deve ser chamada como evento de animação
    public void Attack(){
        // Causa dano de acordo com a arma
        atkTrigger?.Attack();
    }

    // Função que deve ser chamada como evento de animação
    public void EndAttack(){
        atkTrigger?.EndAttack();
        CanMove = true;
    }

    protected void Awake()
    {
        col = GetComponent<Collider2D>();
        movable = GetComponent<Movable>();
        anim = GetComponent<Animator>();
    }

    public void Update(){
        if(invulnerable){
            timer -= Time.deltaTime;
            if(timer <= 0){
                timer = 0f;
                invulnerable = false;
            }
        }
    }

    void Start(){
        Equip(armor);
        Equip(arms);
        Equip(shoes);
    }
}
