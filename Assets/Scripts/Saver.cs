

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public static class Saver
{
    private const string _key = "save";

    [Serializable]
    public class LogicalCylinder
    {
        public float Position;
        public int DependCount;
    }

    public static void Save(List<LogicalCylinder> cylinders)
    {
        //var res = JsonUtility.ToJson(cylinders[0]);
        var res = JsonConvert.SerializeObject(cylinders);
        PlayerPrefs.SetString(_key,res);
        PlayerPrefs.Save();
        Debug.Log("saved");
    }

    public static List<LogicalCylinder> Load()
    {
        var res = PlayerPrefs.GetString(_key);
        return res == null ? null : JsonConvert.DeserializeObject<LogicalCylinder[]>(res).ToList();
    }

}
