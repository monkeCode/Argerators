using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    public void SetMass(float m)
    {
        mass = m;
        mesh.transform.localScale = new Vector3(mesh.transform.localScale.x, Math.Clamp(mass, 0.1f, 2f), mesh.transform.localScale.z);
        mesh.transform.localPosition = new Vector3(mesh.transform.localPosition.x, mass-1, mesh.transform.localPosition.z);
    } 

    public void SetPos(Double pos)
    {
        var p = transform.localPosition;
        p.z = (float)(2*pos-1) * 5;
        transform.localPosition = p;

    }

    private void Update()
    {
        if (text.gameObject.activeSelf)
        {
            text.text = $"{name}\n{GetMass().ToString("F3")}\n{GetPos().ToString("F3")}";
            text.transform.rotation = Camera.main.transform.rotation;
        }
    }
    public double GetPos()
    {
        //return transform.localPosition.z / 10 + 0.5;
        return transform.localPosition.z / 5;
    }


    private void ShowText()
    {
        text.gameObject.SetActive(true);
    }

    private void HideText()
    {
        text.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowText();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideText();
    }
}
