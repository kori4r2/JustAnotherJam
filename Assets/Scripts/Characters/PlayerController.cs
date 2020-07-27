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

    public override void Equip(Armor newArmor){
        racialScores[armor.RacialTrait]--;
        base.Equip(newArmor);
        racialScores[armor.RacialTrait]++;
        UpdateRace();
    }

    public override void Equip(Arms newArms){
        racialScores[arms.RacialTrait]--;
        base.Equip(newArms);
        racialScores[arms.RacialTrait]++;
        UpdateRace();
    }

    public override void Equip(Shoes newShoes){
        racialScores[shoes.RacialTrait]--;
        base.Equip(newShoes);
        racialScores[shoes.RacialTrait]++;
        UpdateRace();
    }

    protected override void Die(){
        if(!invulnerable){
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