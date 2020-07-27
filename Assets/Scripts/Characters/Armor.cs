using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewArmor", menuName ="Armor")]
public class Armor : Equipment
{
    public override TypeEquip Type { get => TypeEquip.Armor; }
    [SerializeField, Range(0, 5f)] private float damageModifier = 1f;
    public float DamageModifier { get => damageModifier; }
    [SerializeField, Range(0, 5f)] private float healthModifier = 1f;
    public float HealthModifier { get => healthModifier; }
}
