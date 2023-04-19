
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DependedCylinder : Cylinder
{
   [SerializeField] private string _formula;

   private ICalculable _function;
   public void ChangePosition(Dictionary<string, double> nameToPos)
   {
      var position = transform.localPosition;
      var res = CalculateFormula(nameToPos);
      res = Math.Clamp(res, -1, 1);
      position.z = 5 * res;
      transform.localPosition = position;
   }

   private float CalculateFormula(Dictionary<string, double> dictionary) => (float) _function.Calculate(dictionary);

   public void SetFormula(string f)
   {
      _formula = f;
      _function = FormulaParser.CreateFunc(f.Replace(" ", ""));
   }

   public string GetFormula() => _formula;
}
