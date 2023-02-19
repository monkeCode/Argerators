
using System;
using System.Collections.Generic;
using UnityEngine;

public class DependedCylinder : Cylinder
{
   enum Functions
   {
      Cos,
      Exp,
      Divide
   }

   private Dictionary<Functions, Func<float, float>> Handler = new()
   {
      {Functions.Cos, f => MathF.Cos(f * MathF.PI * 2)},
      {Functions.Exp, f => MathF.Exp(f)-1},
      {Functions.Divide, f => f / 2}
   };

   [SerializeField] private Functions _currentFunction;
   private float Scale(float x)
   {
      return Handler[_currentFunction](x);
   }
   public void ChangePosition(float pos)
   {
      var position = transform.localPosition;
      var res = Scale(pos);
      if (Mathf.Abs(res) > 1)
         res = (res > 0 ? 1 : -1);
      position.z = 5 * res;
      transform.localPosition = position;
   }
}
