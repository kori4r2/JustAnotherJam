using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeEquip { Armor, Arms, Shoes};

public abstract class Equipment : ScriptableObject
{
    [SerializeField] protected Race racialTrait = Race.Slime;
    public Race RacialTrait { get => racialTrait; }
    public abstract TypeEquip Type { get; }
}
