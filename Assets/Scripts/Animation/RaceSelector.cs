using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class RaceSelector : MonoBehaviour
{
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
    private Transform pivotTransform;

    [SerializeField]
    private Transform modelRoot;
    [SerializeField]
    private Transform modelHead;

    [SerializeField]
    private PartsReferences[] partsReferences;

    [SerializeField]
    private Animator animator;

    public Race CurrentBody 
    {
        get {return (Race) currentBody;}
        set { SetBody(value, currentAtkSpeed); }
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
    private float currentAtkSpeed = 1f;

    private float defaultPositionY;
    private float defaultBonePositionY;

    void Awake()
    {
        if(animator == null)
            animator = GetComponentInChildren<Animator>();
        
        defaultPositionY = modelTransform.localPosition.y;
        // Debug.Log(defaultBonePositionY);

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
        // SetArms(CurrentArms);
        // SetBody(CurrentBody);
        // SetLegs(CurrentLegs);
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
        
        CallUpdateCenter();
        UpdateHead();
    }

    public void SetBody(Race race, float attackSpeed)
    {
        if(race == CurrentBody) return;

        GameObject previousArmor = partsReferences[currentBody].armor;

        if(previousArmor) previousArmor.SetActive(false);

        currentBody = (int) race;
        GameObject currentArmor = partsReferences[currentBody].armor;
        if(currentArmor) currentArmor.SetActive(true);

        animator.SetInteger("BodySelect", currentBody);
        animator.SetFloat("AttackSpeed", attackSpeed);
        
        CallUpdateCenter();
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
        
        CallUpdateCenter();
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
        // Debug.Log("Execute");
    }

    public void CallFinishAttack()
    {
        if(onFinishedAttack != null)
            onFinishedAttack.Invoke();
        // Debug.Log("Finish");
    }

    Coroutine updateCoroutine = null;
    //Move the model so that the center of the character is at local 0
    public void CallUpdateCenter()
    {
        if(updateCoroutine != null)
            StopCoroutine(updateCoroutine);

        updateCoroutine = StartCoroutine(UpdateCenter());
    }
//-5.960464e-08
    private IEnumerator UpdateCenter()
    {
        float counter = 0, difference;

        while(counter < 0.5f)
        {
            difference = FindLocalMiddlePoint() - 0.6881673f;
            modelTransform.localPosition = new Vector3(modelTransform.localPosition.x, defaultPositionY - difference ,modelTransform.localPosition.z);
            counter += Time.deltaTime;
            yield return null;
        }

        difference = FindLocalMiddlePoint() - 0.6881673f;
        modelTransform.localPosition = new Vector3(modelTransform.localPosition.x, defaultPositionY - difference ,modelTransform.localPosition.z);
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

    private float FindLocalMiddlePoint()
    {
        Vector3 localHead = modelTransform.InverseTransformPoint(modelHead.position);
        Vector3 localRoot = modelTransform.InverseTransformPoint(modelRoot.position);

        return 0.5f * (localHead.y + localRoot.y);
    }

    #if UNITY_EDITOR

    [MenuItem("Debug/Setup Human")]
    public static void SetupHuman()
    {
        RaceSelector rc = GameObject.FindObjectOfType<RaceSelector>();

        rc.SetArms(Race.Human);
        rc.SetBody(Race.Human, 1.0f);
        rc.SetLegs(Race.Human);
    }

    [MenuItem("Debug/Setup Elf")]
    public static void SetupElf()
    {
        RaceSelector rc = GameObject.FindObjectOfType<RaceSelector>();

        rc.SetArms(Race.Elf);
        rc.SetBody(Race.Elf, 1.0f);
        rc.SetLegs(Race.Elf);
    }

    [MenuItem("Debug/Setup Orc")]
    public static void SetupOrc()
    {
        RaceSelector rc = GameObject.FindObjectOfType<RaceSelector>();

        rc.SetArms(Race.Orc);
        rc.SetBody(Race.Orc, 1.0f);
        rc.SetLegs(Race.Orc);
    }

    [MenuItem("Debug/Tell me why")]
    public static void FindHeight()
    {
        RaceSelector rc = GameObject.FindObjectOfType<RaceSelector>();

        Debug.Log(rc.FindLocalMiddlePoint());
    }

    [MenuItem("Debug/Setup Slime")]
    public static void SetupSlime()
    {
        RaceSelector rc = GameObject.FindObjectOfType<RaceSelector>();

        rc.SetArms(Race.Slime);
        rc.SetBody(Race.Slime, 1.0f);
        rc.SetLegs(Race.Slime);
    }

    #endif

}
