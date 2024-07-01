using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public static Menu Instance { get; private set; }

    [SerializeField] private GameObject _menu;
    [SerializeField] private GameObject[] _formulaObjects;

    [SerializeField] private TMP_InputField _inputName;
    [SerializeField] private TMP_InputField _formula;
    [SerializeField] private Scrollbar _weightSl;
    [SerializeField] private Scrollbar _posSl;

    private Cylinder _activeCylinder;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowMenu(Cylinder cylinder)
    {
        if (_menu.activeSelf)
            return;

        _menu.SetActive(true);
        _activeCylinder = cylinder;

        var isDep = _activeCylinder is DependedCylinder;

        _inputName.text = _activeCylinder.name;
        _weightSl.value = _activeCylinder.GetMass();
        _posSl.value = _activeCylinder.transform.localPosition.z / 5/2 + 1f;

        foreach (var obj in _formulaObjects)
        {
            obj.SetActive(isDep);
        }

        if (isDep)
        {
            _formula.text = ((DependedCylinder)_activeCylinder).GetFormula();
        }

    }

    public void SetWeight(float value)
    {
        _activeCylinder.SetMass(value);
    }

    public void SetPos(float value)
    {
        _activeCylinder.SetPos(value);
    }

    public void SetFormula(string formula)
    {
        if (_activeCylinder is DependedCylinder dependedCylinder)
        {
            dependedCylinder.SetFormula(formula);
        }
    }

    public void Close()
    {
        _menu.SetActive(false);
    }

    public void SetName(string name)
    {
        _activeCylinder.name = name;
    }
}