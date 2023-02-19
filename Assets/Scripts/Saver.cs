

using System;
using System.Collections.Generic;
using UnityEngine;

public static class Saver
{
    [Serializable]
    public class LogicalCylinder
    {
        public Vector3 Position;
        public List<LogicalCylinder> Depend;
    }
    public static void Save(List<LogicalCylinder> cylinders)
    {
        
    }

    public static void Load()
    {
        
    }

}
