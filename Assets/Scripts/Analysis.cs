using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Analysis : MonoBehaviour
{

    [SerializeField] private GameObject menu;

    [SerializeField] private TextMeshProUGUI globalGCD;
    [SerializeField] private TextMeshProUGUI globalAndness;
    [SerializeField] private TextMeshProUGUI globalOrness;
    [SerializeField] private TextMeshProUGUI stepsCount;
    [SerializeField] private Slider stepsSlider;
    [SerializeField][Range(10, 1000)]
    private int steps = 100;

    private void Start()
    {
        stepsSlider.value = steps;
        stepsCount.text = steps.ToString();
    }

    public void OpenMenu()
    {
        menu.SetActive(true);
    }

    public void CloseMenu()
    {
        menu.SetActive(false);
    }

    public void StepsChanged()
    {
        steps = (int)stepsSlider.value;
        stepsCount.text = steps.ToString();
    }
    public void Approximate()
    {
        CloseMenu();
        StartCoroutine(SimulateCoroutine());
    }

    private IEnumerator SimulateCoroutine()
    {
        float conjuction = 0.0f, disjuction = 0.0f, gcd = 0.0f;
        var cylinders = Panel.Instance.GetCylinders();
        var primary = cylinders.Where(it => it is not DependedCylinder).ToArray();
        for (int i = 0; i < steps; i++)
        {
            foreach(var c in primary)
            {
                c.SetPos(Random.value);
            }
            yield return null;
            var (max, min, g) = Metrics.GetMetrics();
            conjuction += min;
            disjuction += max;
            gcd += g;
        }

        conjuction /= steps;
        disjuction /= steps;
        gcd /= steps;
        var andness = (disjuction - gcd) / (disjuction - conjuction);
        var orness = (gcd-conjuction) / (disjuction - conjuction);
        OpenMenu();
        globalAndness.text = andness.ToString("F3");
        globalOrness.text = orness.ToString("F3");
        globalGCD.text = gcd.ToString("F3");
    }
}
