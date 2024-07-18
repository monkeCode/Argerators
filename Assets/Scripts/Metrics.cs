using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class Metrics : MonoBehaviour
{
    [Header("Scale")]
    [SerializeField] private Transform _textTransform;

    [SerializeField] private TextMeshProUGUI _showableText;

    [SerializeField] private Transform _mark;

    [Header("horizontal bar")]
    [SerializeField] private Transform max;
    [SerializeField] private Transform min;
    [SerializeField] private Transform conjuction;
    [SerializeField] private Transform disjuction;
    [SerializeField] private TextMeshProUGUI maxTextBar;
    [SerializeField] private TextMeshProUGUI minTextBar;
    [SerializeField] private TextMeshProUGUI gcdTextBar;
    [Header("metrics")]
    [SerializeField] private Transform gcd;
    [SerializeField] private TextMeshProUGUI ornessText;
    [SerializeField] private TextMeshProUGUI andnessText;
    [SerializeField] private TextMeshProUGUI gcdText;

    private float _barTollerance = 0.01f;
    // Update is called once per frame
    void Update()
    {
        UpdateScale();
        UpdateGCD();
    }

    private void UpdateScale()
    {
        transform.position = new Vector3(_mark.transform.position.x - 1, transform.position.y, transform.position.z);

        var angle = Panel.Instance.GetAngle();
        _showableText.text = angle.ToString("F3");
        _textTransform.localPosition =
            new Vector3(_textTransform.localPosition.x, angle * 6 - 3, _textTransform.localPosition.z);
    }

    private static float GetGCD(float conj, float disj) =>
        (RadioSelector.Instanse.value) switch
        {
            "Arithmetic mean" => (conj + disj) / 2,
            "Geometric mean" => Mathf.Sqrt(conj*disj),
            "Max" => disj,
            "Min" => conj,
            "Simulation" => Panel.Instance.GetAngle(),
            _ => throw new ArgumentOutOfRangeException()
        };

    /// <summary>
    ///
    /// </summary>
    /// <returns>
    /// Touple of max, min, gcd
    /// </returns>
    public  static (float,float,float) GetMetrics()
    {
        var positions = Panel.Instance.GetCylinders().Where(it => it is not DependedCylinder).Select(it=> (float)it.GetPos()).ToList();

        var conj = positions.Min();
        var disj = positions.Max();
        var gcd = GetGCD(conj, disj);
        return (disj, conj, gcd);
    }

    private void UpdateGCD()
    {
        var positions = Panel.Instance.GetCylinders().Where(it => it is not DependedCylinder).Select(it=> (float)it.GetPos()).ToList();
        if (positions.Count() < 2)
            return;

        var conj = positions.Min();
        var disj = positions.Max();
        var dist = (disj - conj);
        var gcd = GetGCD(conj, disj);

        conjuction.position =
            new Vector3(conjuction.position.x, (max.position.y - min.position.y) * conj + min.position.y, conjuction.position.z);

        minTextBar.transform.position = new Vector3(minTextBar.transform.position.x,
            (max.position.y - min.position.y) * conj + min.position.y, minTextBar.transform.position.z);

        disjuction.position =
            new Vector3(disjuction.position.x, (max.position.y - min.position.y) * disj + min.position.y,
                disjuction.position.z);

        maxTextBar.transform.position = new Vector3(maxTextBar.transform.position.x,
            (max.position.y - min.position.y) * disj + min.position.y, maxTextBar.transform.position.z);

        this.gcd.position =
            new Vector3(this.gcd.position.x, (max.position.y - min.position.y) * gcd + min.position.y, this.gcd.position.z);
        gcdTextBar.transform.position = new Vector3(gcdTextBar.transform.position.x,
            (max.position.y - min.position.y) * gcd + min.position.y, gcdTextBar.transform.position.z);

        if (MathF.Abs(gcd - disj) < _barTollerance && MathF.Abs(gcd - conj) < _barTollerance)
        {
            gcdTextBar.text = "gcd, max, min: " + gcd.ToString("F3");
            maxTextBar.text = "";
            minTextBar.text = "";
        }
        else if (MathF.Abs(gcd - disj) < _barTollerance)
        {
            gcdTextBar.text = "gcd, max: " + gcd.ToString("F3");
            maxTextBar.text = "";
            minTextBar.text = "min: " + conj.ToString("F3");
        }
        else if (MathF.Abs(gcd - conj) < _barTollerance)
        {
            gcdTextBar.text = "gcd, min: " + gcd.ToString("F3");
            maxTextBar.text = "max: " + disj.ToString("F3");
            minTextBar.text = "";
        }
        else if (MathF.Abs(disj - conj) < _barTollerance)
        {
            gcdTextBar.text = "gcd: " + gcd.ToString("F3");
            maxTextBar.text = "max, min: " + conj.ToString("F3");
            minTextBar.text = "";
        }
        else
        {
            maxTextBar.text = "max: " + disj.ToString("F3");
            gcdTextBar.text = "gcd: " + gcd.ToString("F3");
            minTextBar.text = "min: " + conj.ToString("F3");
        }

        gcdText.text = "GCD◊:\n" + gcd.ToString("F3");
        ornessText.text = "Andness:\n" +  ((disj-gcd)/dist).ToString("F3");
        andnessText.text = "Orness:\n" + ((gcd-conj)/dist).ToString("F3");
    }
}
