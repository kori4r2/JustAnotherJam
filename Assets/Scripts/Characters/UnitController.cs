using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    protected const int baseSpeed = 8;

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
    public float AttackSpeed { get; protected set; } = 1.0f;

    protected Collider2D col;
    protected Movable movable;
    protected Animator anim;
    public virtual Vector2 Direction { get => movable.Direction; }
    public bool CanMove { get => movable.CanMove; set => movable.CanMove = value; }
    [SerializeField, Range(0f, 5f)] protected float invulnerableTime = 1f;
    [SerializeField] protected RaceSelector raceSelector;
    [SerializeField] protected Armor armor;
    [SerializeField] protected Arms arms;
    [SerializeField] protected Shoes shoes;
    protected AttackTrigger atkTrigger = null;
    protected float timer = 0f;
    public bool invulnerable = false;
    private bool attacking = false;

    public delegate void HealthCallback(float percentHealth);

    public HealthCallback OnHealthChange = null;

    protected virtual void EquipArmor(Armor newArmor){
        armor = newArmor;
        raceSelector?.SetBody(armor.RacialTrait);
        Damage = Mathf.FloorToInt(baseDamage * newArmor.DamageModifier);
        float hpPercentage = CurHP / (1.0f * MaxHP);
        MaxHP = Mathf.FloorToInt(baseHP * newArmor.HealthModifier);
        CurHP = Mathf.Max(Mathf.FloorToInt(hpPercentage * MaxHP), 1);
    }

    protected virtual void EquipWeapon(Arms newArms){
        arms = newArms;
        raceSelector?.SetArms(arms.RacialTrait);
        if(atkTrigger != null){
            Destroy(atkTrigger.gameObject);
        }
        atkTrigger = arms.AddTrigger(transform);
    }

    protected virtual void EquipShoes(Shoes newShoes){
        shoes = newShoes;
        AttackSpeed = shoes.AttackSpeedModifier;
        raceSelector?.SetLegs(shoes.RacialTrait, AttackSpeed);
        Speed = Mathf.FloorToInt(baseSpeed * shoes.SpeedModifier);

    }

    public virtual void TakeDamage(UnitController attacker, float multiplier = 1f){
        if(!invulnerable){
            // Debug.Log(attacker.name + " damaged " + name);
            CurHP -= Mathf.FloorToInt(attacker.Damage * multiplier);
            if(CurHP <= 0){
                Die();
            }else{
                timer = invulnerableTime;
                invulnerable = true;
            }
            CallUpdateHealth();
        }
    }

    protected virtual void Die(){
        Destroy(gameObject);
    }

    protected void StartAttack(){
        if(CanMove){
            CanMove = false;
            atkTrigger?.PrepareAttack(this);
            raceSelector?.StartAttackAnimation();
        }
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
        timer = 0f;
    }

    private void UpdateRotation(Vector2 direction){
        Vector3 currentAngles = raceSelector.transform.localEulerAngles;
        if(direction == Vector2.down){
            raceSelector.transform.localEulerAngles = new Vector3(currentAngles.x, 180f, currentAngles.z);
        }else if(direction == Vector2.up){
            raceSelector.transform.localEulerAngles = new Vector3(currentAngles.x, 0f, currentAngles.z);
        }else if(direction == Vector2.right){
            raceSelector.transform.localEulerAngles = new Vector3(currentAngles.x, 90f, currentAngles.z);
        }else if(direction == Vector2.left){
            raceSelector.transform.localEulerAngles = new Vector3(currentAngles.x, -90f, currentAngles.z);
        }
    }

    public void Update(){
        if(raceSelector && CanMove)
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
        EquipArmor(armor);
        EquipWeapon(arms);
        EquipShoes(shoes);
        if(raceSelector){
            raceSelector.onExecuteAttack.AddListener(Attack);
            raceSelector.onFinishedAttack.AddListener(EndAttack);
            raceSelector.CallUpdateCenter();
        }
    }

    protected void CallUpdateHealth()
    {
        float hpPercentage = CurHP / (1.0f * MaxHP);
        if(OnHealthChange != null)
            OnHealthChange(hpPercentage);
    }
}
