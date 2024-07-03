using System;
using System.Collections.Generic;
using System.Linq;
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
        public float Mass;
    }
    [Serializable]
    public class DependedLogicalCylinder: LogicalCylinder
    {
        public string Formula;
    }
    
    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }

    public static void Save(List<LogicalCylinder> cylinders)
    {
        var wrapper = new Wrapper<LogicalCylinder>
        {
            Items = cylinders.ToArray()
        };
        var res = JsonUtility.ToJson(wrapper);
        PlayerPrefs.SetString(_key,res);
        PlayerPrefs.Save();
        Debug.Log("saved");
    }

    public static void Save(List<DependedLogicalCylinder> cylinders)
    {
        var wrapper = new Wrapper<DependedLogicalCylinder>
        {
            Items = cylinders.ToArray()
        };
        var res = JsonUtility.ToJson(wrapper);
        PlayerPrefs.SetString(_key2,res);
        PlayerPrefs.Save();
        Debug.Log("saved");
    }

    public static List<LogicalCylinder> Load()
    {
        var res = PlayerPrefs.GetString(_key, defaultValue:null);
        return res == null ? null :  JsonUtility.FromJson<Wrapper<LogicalCylinder>>(res).Items.ToList();
    }

    public static List<DependedLogicalCylinder> LoadDependedCylinders()
    {
        var res = PlayerPrefs.GetString(_key2);
        return res == null ? null :  JsonUtility.FromJson<Wrapper<DependedLogicalCylinder>>(res).Items.ToList();
    }

}
