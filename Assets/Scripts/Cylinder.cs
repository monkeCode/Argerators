using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class Cylinder : MonoBehaviour
{

    [SerializeField] protected float _mass = 1;
    public string Name;
    public float GetMass() => _mass;
    public void SetMass(float m)
    {
        _mass = m;
        transform.localScale = new Vector3(transform.localScale.x, Math.Clamp(_mass, 0.1f, 2f), transform.lossyScale.z);
    } 

    public void SetPos(float pos)
    {
        var p = transform.localPosition;
        p.z = pos * 5;
        transform.localPosition = p;

    }
}
