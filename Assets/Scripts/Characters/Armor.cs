using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewArmor", menuName ="Armor")]
public class Armor : ScriptableObject
{
    [SerializeField, Range(0, 5f)] private float attackModifier = 1f;
    public float AttackModifier { get => attackModifier; }
    [SerializeField, Range(0, 5f)] private float healthModifier = 1f;
    public float HealthModifier { get => healthModifier; }
    [SerializeField, Range(0, 5f)] private float speedModifier = 1f;
    public float SpeedModifier { get => speedModifier; }
}
