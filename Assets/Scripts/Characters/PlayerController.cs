using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(MoveToInput))]
public class PlayerController : UnitController
{
    public static PlayerController Instance { get; private set; } = null;

    public Race CurRace { get; private set; } = Race.Slime;
    private Dictionary<Race, int> racialScores;
    [SerializeField] private Navigator navigator = null;

    public void StartGame(){
        gameObject.tag = "Player";
        CanMove = true;
    }

    new void Awake(){
        if(Instance == null){
            Instance = this;

            base.Awake();
            racialScores = new Dictionary<Race, int>();
            racialScores.Add(Race.Slime, 3);
            racialScores.Add(Race.Elf, 0);
            racialScores.Add(Race.Human, 0);
            racialScores.Add(Race.Orc, 0);
            CurRace = Race.Slime;
            navigator.Player = this;
            movable.NextPosition = GetComponent<MoveToInput>();
            CanMove = false;
        }else{
            Debug.Log("Tentou instanciar mais de um PlayerController");
            Destroy(gameObject);
        }
    }

    public void OnDestroy(){
        if(Instance == this){
            Instance = null;
        }
    }

    public void Equip(Equipment equipment){
        if(equipment == null) return;

        switch(equipment.Type){
            case TypeEquip.Armor:
                Armor newArmor = equipment as Armor;
                EquipArmor(newArmor);
                break;
            case TypeEquip.Arms:
                Arms newArms = equipment as Arms;
                EquipWeapon(newArms);
                break;
            case TypeEquip.Shoes:
                Shoes newShoes = equipment as Shoes;
                EquipShoes(newShoes);
                break;
        }
    }

    protected override void EquipArmor(Armor newArmor){
        racialScores[armor.RacialTrait]--;
        base.EquipArmor(newArmor);
        racialScores[armor.RacialTrait]++;
        UpdateRace();
    }

    protected override void EquipWeapon(Arms newArms){
        racialScores[arms.RacialTrait]--;
        base.EquipWeapon(newArms);
        racialScores[arms.RacialTrait]++;
        UpdateRace();
    }

    protected override void EquipShoes(Shoes newShoes){
        racialScores[shoes.RacialTrait]--;
        base.EquipShoes(newShoes);
        racialScores[shoes.RacialTrait]++;
        UpdateRace();
    }

    public void ForceDie(){
        invulnerable = true;
        timer = float.MaxValue;
        Destroy(raceSelector.gameObject);
        if(atkTrigger){
            Destroy(atkTrigger.gameObject);
        }
        raceSelector = null;
        Debug.Log("IsDead");
        StartCoroutine(DelayStartGame());
    }

    protected override void Die(){
        if(!invulnerable){
            ForceDie();
        }
    }

    private IEnumerator DelayStartGame(){
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void UpdateRace(){
        foreach(Race race in racialScores.Keys){
            if(racialScores[race] >= 2){
                CurRace = race;
                return;
            }
        }
        CurRace = Race.Slime;
    }

    public new void Update(){
        base.Update();
        // yes, it's jump, shut up
        if(CanMove && Input.GetButton("Jump")){
            StartAttack();
        }
    }

    public void CurePlayer(float percent)
    {
        float hpPercentage = CurHP / (1.0f * MaxHP);
        hpPercentage += percent;

        hpPercentage = Mathf.Clamp01(hpPercentage);

        CurHP = Mathf.Max(Mathf.FloorToInt(hpPercentage * MaxHP), 1);

        CallUpdateHealth();
    }
}