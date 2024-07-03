using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Cylinder : MonoBehaviour
{

    [SerializeField] protected float mass = 1;
    public string name;
    public float GetMass() => mass;
    public void SetMass(float m)
    {
        mass = m;
        transform.localScale = new Vector3(transform.localScale.x, Math.Clamp(mass, 0.01f, 2f), transform.localScale.z);
        transform.localPosition = new Vector3(transform.localPosition.x, mass, transform.localPosition.z);
    } 

    public void SetPos(float pos)
    {
        var p = transform.localPosition;
        p.z = (2*pos-1) * 5;
        transform.localPosition = p;

    }

    public double GetPos()
    {
        return transform.localPosition.z / 10 + 0.5;
    }
}
