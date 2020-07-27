using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item")]
public class ProtectedItem : RoomObject
{
    // Precisa ter uma referencia ao objeto do item para saber se ele ja foi coletado
    private ItemObject item;

    public override void Spawn(Transform parent, Vector3 position, Quaternion rotation){
        base.Spawn(parent, position, rotation);
        item = obj.GetComponent<ItemObject>();
    }

    public override void Enable(){
        base.Enable();
        item?.Enable();
    }

    public override void Disable(){
        item?.Disable();
        base.Disable();
    }
}
