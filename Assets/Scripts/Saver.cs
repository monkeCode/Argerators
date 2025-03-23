using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Saver
{
    private const string _key = "save";

    [Serializable]
    public class LogicalCylinder
    {
        public double Position;
        public string Name;
        public float Mass;
        public Color color;
    }

    [Serializable]
    public class DependedLogicalCylinder : LogicalCylinder
    {
        public string Formula;
    }

    [Serializable]
    private class Wrapper
    {
        public List<SerializedCylinder> Items;
    }

    [Serializable]
    private class SerializedCylinder
    {
        public string Type; // Тип объекта (LogicalCylinder или DependedLogicalCylinder)
        public string Data; // Сериализованные данные объекта
    }

    public static void Save(List<LogicalCylinder> cylinders)
    {
        var wrapper = new Wrapper
        {
            Items = cylinders.Select(cyl => new SerializedCylinder
            {
                Type = cyl is DependedLogicalCylinder ? typeof(DependedLogicalCylinder).FullName : typeof(LogicalCylinder).FullName,
                Data = JsonUtility.ToJson(cyl)
            }).ToList()
        };

        var res = JsonUtility.ToJson(wrapper);
        PlayerPrefs.SetString(_key, res);
        PlayerPrefs.Save();
        Debug.Log("saved");
    }

    public static List<LogicalCylinder> Load()
    {
        var res = PlayerPrefs.GetString(_key, defaultValue: null);
        if (res == null) return null;

        var wrapper = JsonUtility.FromJson<Wrapper>(res);
        var cylinders = new List<LogicalCylinder>();

        foreach (var item in wrapper.Items)
        {
            if (item.Type == typeof(LogicalCylinder).FullName)
            {
                cylinders.Add(JsonUtility.FromJson<LogicalCylinder>(item.Data));
            }
            else if (item.Type == typeof(DependedLogicalCylinder).FullName)
            {
                cylinders.Add(JsonUtility.FromJson<DependedLogicalCylinder>(item.Data));
            }
        }

        return cylinders;
    }
}