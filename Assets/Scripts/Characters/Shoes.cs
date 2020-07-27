using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewShoes", menuName ="Shoes")]
public class Shoes : Equipment
{
    public override TypeEquip Type { get => TypeEquip.Shoes; }

    [SerializeField, Range(0, 5f)] private float speedModifier = 1f;
    public float SpeedModifier { get => speedModifier; }
    [SerializeField, Range(0, 5f)] private float attackSpeedModifier = 1f;
    public float AttackSpeedModifier { get => attackSpeedModifier; }
}
