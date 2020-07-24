using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveToInput))]
public class PlayerController : UnitController
{
    public override void Equip(Armor newArmor){
        base.Equip(newArmor);
        UpdateRace();
    }

    public override void Equip(Arms newArms){
        base.Equip(newArms);
        UpdateRace();
    }

    public override void Equip(Shoes newShoes){
        base.Equip(newShoes);
        UpdateRace();
    }

    private void UpdateRace(){
    }
}
