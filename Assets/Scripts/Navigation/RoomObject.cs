using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoomObject : ScriptableObject
{
    [SerializeField] protected GameObject prefab;
    protected GameObject obj;

    public virtual void Spawn(Transform parent, Vector3 position, Quaternion rotation){
        obj = GameObject.Instantiate(prefab, position, rotation, parent);
    }

    public virtual void Enable(){
        obj.SetActive(true);
    }

    public virtual void Disable(){
        obj.SetActive(false);
    }
}
