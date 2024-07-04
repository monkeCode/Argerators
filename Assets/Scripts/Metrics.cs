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

    [Header("GCD and other")]
    [SerializeField] private Transform max;
    [SerializeField] private Transform min;
    [SerializeField] private Transform conjuction;
    [SerializeField] private Transform disjuction;
    [SerializeField] private Transform gcd;
    [SerializeField] private TextMeshProUGUI ornessText;
    [SerializeField] private TextMeshProUGUI andnessText;
    [SerializeField] private TextMeshProUGUI gcdText;
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

    private float GetGCD(float conj, float disj) =>
        (RadioSelector.Instanse.value) switch
        {
            "Arithmetic mean" => (conj + disj) / 2,
            "Geometric mean" => Mathf.Sqrt(conj*disj),
            "Max" => disj,
            "Min" => conj

        };

    private void UpdateGCD()
    {
        var positions = Panel.Instance.GetCylinders().Select(it=> (float)it.GetPos());
        if (positions.Count() < 2)
            return;

        var conj = positions.Min();
        var disj = positions.Max();
        var dist = (disj - conj);
        var gcd = GetGCD(conj, disj);

        conjuction.position =
            new Vector3(conjuction.position.x, (max.position.y - min.position.y) * conj + min.position.y, conjuction.position.z);
        disjuction.position =
            new Vector3(disjuction.position.x, (max.position.y - min.position.y) * disj + min.position.y,
                disjuction.position.z);
        this.gcd.position =
            new Vector3(this.gcd.position.x, (max.position.y - min.position.y) * gcd + min.position.y, this.gcd.position.z);

        gcdText.text = "GCD◊:\n" + gcd.ToString("F3");
        ornessText.text = "Andness:\n" +  ((disj-gcd)/dist).ToString("F3");
        andnessText.text = "Orness:\n" + ((gcd-conj)/dist).ToString("F3");
    }
}
