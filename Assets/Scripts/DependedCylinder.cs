
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DependedCylinder : Cylinder
{
   [SerializeField] private string _formula;
   public void ChangePosition(Dictionary<string, float> nameToPos)
   {
      var position = transform.localPosition;
      var res = CalculateFormula(nameToPos);
      res = Math.Clamp(res, -1, 1);
      position.z = 5 * res;
      transform.localPosition = position;
   }

   private float CalculateFormula(Dictionary<string, float> dictionary)
   {
      StringBuilder adaptedFormula = new StringBuilder(_formula);
      foreach (var n in dictionary.Keys)
      {
         adaptedFormula =  adaptedFormula.Replace(n, dictionary[n].ToString());
      }

      return (float) FormulaParser.Calculate(adaptedFormula.Replace(" ", "").ToString());
   }

   public void SetFormula(string f) => _formula = f;
}
