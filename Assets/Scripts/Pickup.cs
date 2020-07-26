using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public enum TypeEquip { Armor, Arms, Shoes};

    public TypeEquip typeEquip;
    public Armor armorEquip;
    public Arms armsEquip;
    public Shoes shoesEquip;

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            PlayerController pc = col.GetComponent<PlayerController>();

            if(pc != null)
            {
                switch(typeEquip)
                {
                    case TypeEquip.Armor:
                        pc.Equip(armorEquip);
                        break;
                    case TypeEquip.Arms:
                        pc.Equip(armsEquip);
                        break;
                    case TypeEquip.Shoes:
                        pc.Equip(shoesEquip);
                        break;
                }
            }
        }
    }
}
