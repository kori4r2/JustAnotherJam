using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewShoes", menuName ="Shoes")]
public class Shoes : ScriptableObject
{
    [SerializeField] private Race racialTrait = Race.Slime;
    public Race RacialTrait { get => racialTrait; }
}
