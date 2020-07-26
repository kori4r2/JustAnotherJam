using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Race{
    Slime = 0,
    Human = 1,
    Elf = 2,
    Orc = 3
}

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
    [SerializeField] private RaceSelector raceSelector;
    [SerializeField] protected Armor armor;
    [SerializeField] protected Arms arms;
    [SerializeField] protected Shoes shoes;
    private AttackTrigger atkTrigger = null;
    private float timer = 0f;
    private bool invulnerable = false;
    private bool attacking = false;

    public virtual void Equip(Armor newArmor){
        armor = newArmor;
        raceSelector?.SetBody(armor.RacialTrait);
        Damage = Mathf.FloorToInt(baseDamage * newArmor.DamageModifier);
        Speed = Mathf.FloorToInt(baseSpeed * newArmor.SpeedModifier);
        float hpPercentage = MaxHP / (1.0f * CurHP);
        MaxHP = Mathf.FloorToInt(baseHP * newArmor.HealthModifier);
        CurHP = Mathf.Max(Mathf.FloorToInt(hpPercentage * MaxHP), 1);
    }

    public virtual void Equip(Arms newArms){
        arms = newArms;
        raceSelector?.SetArms(arms.RacialTrait);
        if(atkTrigger){
            Destroy(atkTrigger.gameObject);
            atkTrigger = arms.AddTrigger(transform);
        }
    }

    public virtual void Equip(Shoes newShoes){
        shoes = newShoes;
        raceSelector?.SetLegs(shoes.RacialTrait);
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
        raceSelector?.StartAttackAnimation();
    }

    // Função que deve ser chamada como evento de animação
    public void Attack(){
        // Gambiarra pq o evento ta sendo chamado 3 vezes
        if(!attacking){
            attacking = true;
            // Causa dano de acordo com a arma
            atkTrigger?.Attack();
        }
    }

    // Função que deve ser chamada como evento de animação
    public void EndAttack(){
        // Gambiarra pq o evento ta sendo chamado 3 vezes
        if(!CanMove && attacking){
            attacking = false;
            atkTrigger?.EndAttack();
            CanMove = true;
        }
    }

    protected void Awake()
    {
        col = GetComponent<Collider2D>();
        movable = GetComponent<Movable>();
        anim = GetComponent<Animator>();
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

    public void Update(){
        UpdateRotation(Direction);
        raceSelector?.SetAnimationSpeed(movable.CurrentSpeed.magnitude);
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
        if(raceSelector){
            raceSelector.onExecuteAttack.AddListener(Attack);
            raceSelector.onFinishedAttack.AddListener(EndAttack);
        }
    }
}
