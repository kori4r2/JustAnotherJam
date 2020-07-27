﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewShoes", menuName ="Shoes")]
public class Shoes : Equipment
{
    public override TypeEquip Type { get => TypeEquip.Shoes; }
}
