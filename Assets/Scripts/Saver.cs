

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public static class Saver
{
    private const string _key = "save";
    private const string _key2 = "depSave";

    [Serializable]
    public class LogicalCylinder
    {
        public float Position;
        public string Name;
    }
    public class DependedLogicalCylinder: LogicalCylinder
    {
        public string Formula;
    }

    public static void Save(List<LogicalCylinder> cylinders)
    {
        var res = JsonConvert.SerializeObject(cylinders);
        PlayerPrefs.SetString(_key,res);
        PlayerPrefs.Save();
        Debug.Log("saved");
    }

    public static void Save(List<DependedLogicalCylinder> cylinders)
    {
        var res = JsonConvert.SerializeObject(cylinders);
        PlayerPrefs.SetString(_key2,res);
        PlayerPrefs.Save();
        Debug.Log("saved");
    }

    public static List<LogicalCylinder> Load()
    {
        var res = PlayerPrefs.GetString(_key);
        return res == null ? null : JsonConvert.DeserializeObject<LogicalCylinder[]>(res).ToList();
    }

}
