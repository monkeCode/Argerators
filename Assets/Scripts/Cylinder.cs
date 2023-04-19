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
    public void SetMass(float m) => _mass = m;
}
