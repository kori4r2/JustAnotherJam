using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitWeaponToHand : MonoBehaviour
{
    public Transform handTranform;

    public Vector3 positionOffset;
    public Vector3 rotationOffset;

    private Transform myTransform;


    void Awake()
    {
        myTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        myTransform.position = handTranform.position + handTranform.rotation * positionOffset;
        myTransform.rotation = handTranform.rotation * Quaternion.Euler(rotationOffset);
        // myTransform.localRotation = Quaternion.Euler(rotationOffset);
    }
}
