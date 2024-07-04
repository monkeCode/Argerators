using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class RadioSelector : MonoBehaviour
{
    // Start is called before the first frame update
    public static RadioSelector Instanse {
        get;
    private set; }

    public string value { get; private set; }

    private List<String> _variants;
    private void Awake()
    {
        Instanse = this;
    }

    void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        RadioButtonGroup group = root.Q<RadioButtonGroup>("gcd");

        _variants = group.choices.ToList();
        value = "Arithmetic mean";
        group.RegisterValueChangedCallback(evt =>
        {
            if (evt.newValue == -1)
            {
                return;
            }

            value = _variants[evt.newValue];
        });
    }

}
