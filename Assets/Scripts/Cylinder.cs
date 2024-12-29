using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class Cylinder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] protected float mass = 1;
    [SerializeField] protected TextMeshProUGUI text;
    [SerializeField] protected GameObject mesh;
    public string name;
    public float GetMass() => mass;

    private bool _mouseEntered = false;
    public void SetMass(float m)
    {
        m = Mathf.Clamp(m, 0,1);
        mass = m;
        mesh.transform.localScale = new Vector3(mesh.transform.localScale.x, Math.Clamp(mass, 0.1f, 2f), mesh.transform.localScale.z);
        mesh.transform.localPosition = new Vector3(mesh.transform.localPosition.x, mass-1, mesh.transform.localPosition.z);
    } 

    public void SetPos(double pos)
    {
        var p = transform.localPosition;
        p.z = (float)Math.Clamp(pos, -1,1) * 5;
        transform.localPosition = p;

    }

    private void Start()
    {
        text.gameObject.SetActive(true);
    }
    private void Update()
    {
        if (_mouseEntered)
        {
            text.text = $"{name}\nw {GetMass().ToString("F3")}\nz {GetPos().ToString("F3")}";
        }
        else
        {
            text.text = $"{name}";
        }
        text.transform.rotation = Camera.main.transform.rotation;
    }
    public double GetPos()
    {
        //return transform.localPosition.z / 10 + 0.5;
        return transform.localPosition.z / 5;
    }


    private void ShowText()
    {
        //text.gameObject.SetActive(true);
        _mouseEntered = true;
    }

    private void HideText()
    {
        //text.gameObject.SetActive(false);
        _mouseEntered = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowText();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideText();
    }

    public virtual void SetColor(Color color)
    {
        mesh.GetComponent<Renderer>().material.SetColor("_Color", color);
    }

    public virtual Color GetColor()
    {
        return mesh.GetComponent<Renderer>().material.GetColor("_Color");
    }
}
