using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewArms", menuName ="Arms")]
public class Arms : ScriptableObject
{
    [SerializeField] private Race racialTrait = Race.Slime;
    public Race RacialTrait { get => racialTrait; }
}
