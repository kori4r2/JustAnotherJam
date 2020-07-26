using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class RaceSelector : MonoBehaviour
{
    public enum Race
    {
        Slime = 0,
        Human = 1,
        Elf = 2,
        Orc = 3
    }

    [System.Serializable]
    public class PartsReferences
    {
        public GameObject boots;
        public GameObject armor;
        public GameObject weapon;
        public GameObject bodyPart;
    }
    
    [Header("References")]
    [SerializeField]
    private Transform modelTransform;

    [SerializeField]
    private PartsReferences[] partsReferences;

    [SerializeField]
    private Animator animator;

    public Race CurrentBody 
    {
        get {return (Race) currentBody;}
        set { SetBody(value); }
    }

    public Race CurrentArms 
    {
        get {return (Race) currentArms;}
        set { SetArms(value); }
    }

    public Race CurrentLegs 
    {
        get {return (Race) currentLegs;}
        set { SetLegs(value); }
    }

    public Race CurrentHead
    {
        get {return (Race) currentHead;}
    }

    [Header("Events")]
    public UnityEvent onExecuteAttack = new UnityEvent();
    public UnityEvent onFinishedAttack = new UnityEvent();

    private int currentHead = 0;
    private int currentLegs = 0;
    private int currentBody = 0;
    private int currentArms = 0;

    void Awake()
    {
        if(animator == null)
            animator = GetComponentInChildren<Animator>();
        
        foreach (var item in partsReferences)
        {
            if(item.boots)
                item.boots.SetActive(false);
            if(item.armor)
                item.armor.SetActive(false);
            if(item.weapon)
                item.weapon.SetActive(false);
            if(item.bodyPart)
                item.bodyPart.SetActive(false);
        }
    }

    void Start()
    {
        SetArms(CurrentArms);
        SetBody(CurrentBody);
        SetLegs(CurrentLegs);
    }

    public void SetArms(Race race)
    {
        if(race == CurrentArms) return;

        GameObject previousWeapon = partsReferences[currentArms].weapon;

        if(previousWeapon) previousWeapon.SetActive(false);

        currentArms = (int) race;
        GameObject currentWeapon = partsReferences[currentArms].weapon;
        if(currentWeapon) currentWeapon.SetActive(true);

        animator.SetInteger("ArmSelect",currentArms);
        
        UpdateHead();
    }

    public void SetBody(Race race)
    {
        if(race == CurrentBody) return;

        GameObject previousArmor = partsReferences[currentBody].armor;

        if(previousArmor) previousArmor.SetActive(false);

        currentBody = (int) race;
        GameObject currentArmor = partsReferences[currentBody].armor;
        if(currentArmor) currentArmor.SetActive(true);

        animator.SetInteger("BodySelect", currentBody);
        
        UpdateHead();
    }

    public void SetLegs(Race race)
    {
        if(race == CurrentLegs) return;

        GameObject previousBoots = partsReferences[currentLegs].boots;

        if(previousBoots) previousBoots.SetActive(false);

        currentLegs = (int) race;
        GameObject currentBoots = partsReferences[currentLegs].boots;
        if(currentBoots) currentBoots.SetActive(true);

        animator.SetInteger("LegSelect", currentLegs);
        
        UpdateHead();
    }

    public void StartAttackAnimation()
    {
        animator.SetTrigger("Attack");
    }

    public void SetAnimationSpeed(float speed)
    {
        animator.SetFloat("Speed", speed);
    }

    public void SetAnimationDie()
    {
        animator.SetTrigger("Death");
    }

    public void SetAnimationReapear()
    {
        animator.SetTrigger("Reapear");
    }
    
    public void CallExecuteAttack()
    {
        if(onExecuteAttack != null)
            onExecuteAttack.Invoke();
        Debug.Log("Execute");
    }

    public void CallFinishAttack()
    {
        if(onFinishedAttack != null)
            onFinishedAttack.Invoke();
        Debug.Log("Finish");
    }


    private void UpdateHead()
    {
        int[] matches = new int[4];

        matches[currentBody]++;
        matches[currentLegs]++;
        matches[currentArms]++;

        int highestId = 0;
        int highestCount = 0;

        for (int i = 0; i < matches.Length; i++)
        {
            if(matches[i] > highestCount)
            {
                highestCount = matches[i];
                highestId = i;
            }
        }

        if(highestCount > 1)
        {

            GameObject previousPart = partsReferences[currentHead].bodyPart;

            if(previousPart) previousPart.SetActive(false);

            currentHead = highestId;

            GameObject currentPart = partsReferences[currentHead].bodyPart;
            if(currentPart) currentPart.SetActive(true);
        }

    }

    #if UNITY_EDITOR

    [MenuItem("Debug/Setup Human")]
    public static void SetupHuman()
    {
        RaceSelector rc = GameObject.FindObjectOfType<RaceSelector>();

        rc.SetArms(Race.Human);
        rc.SetBody(Race.Human);
        rc.SetLegs(Race.Human);
    }

    [MenuItem("Debug/Setup Elf")]
    public static void SetupElf()
    {
        RaceSelector rc = GameObject.FindObjectOfType<RaceSelector>();

        rc.SetArms(Race.Elf);
        rc.SetBody(Race.Elf);
        rc.SetLegs(Race.Elf);
    }

    [MenuItem("Debug/Setup Orc")]
    public static void SetupOrc()
    {
        RaceSelector rc = GameObject.FindObjectOfType<RaceSelector>();

        rc.SetArms(Race.Orc);
        rc.SetBody(Race.Orc);
        rc.SetLegs(Race.Orc);
    }

    #endif

}
