using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Panel : MonoBehaviour
{

    [SerializeField][Range(-180,180)] private float _angle;
    [SerializeField] private Cylinder _cylinder;
    [SerializeField] private DependedCylinder _dependedCylinder;
    [SerializeField] private Transform _plane;
    [SerializeField] private float _maxAngleOffset = 23.20f;

    private List<Cylinder> _cylinders = new();
    
    private void Update()
    {
        var depCyls = _cylinders.OfType<DependedCylinder>().ToList();
        var dict = GetCylPositions();
        foreach (var c in depCyls)
        {
            c.ChangePosition(dict);
        }
        
        var angle = CalculateAngle();
        angle = Math.Clamp(angle, -_maxAngleOffset, _maxAngleOffset);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(angle,0,0),0.1f);
    }

    private float CalculateAngle()
    {
        var angle = 0.0f;
        foreach (var cyl in _cylinders)
        {
            angle += (cyl.transform.position.z - transform.position.z) * cyl.GetMass();
        }
        return angle;
    }
    
    [ContextMenu("Angle")]
    private void UpdateAngle()
    {
        transform.rotation = Quaternion.Euler(_angle,0,0);
    }

    private Dictionary<string, float> GetCylPositions()=> 
        _cylinders.ToDictionary(cylinder => cylinder.Name, cylinder=>cylinder.transform.localPosition.z/5);

    private void Resize()
    {
        var mid = (float)_cylinders.Count / 2;
        for(int i = 0; i < _cylinders.Count; i++)
        {
            var pos = _cylinders[i].transform.localPosition;
            pos.y = 1;
            pos.x = (i - mid) * 2 + 1f;
            _cylinders[i].transform.localPosition = pos;
        }
        _plane.localScale = new Vector3(_cylinders.Count/5.0f, 1, 1);
    }

    private void Start()
    {
        var data = Saver.Load();
        if(data == null) return;

        foreach (var cyl in data)
        {
            AddNewCylinder(cyl);
        }

        var depDate = Saver.LoadDependedCylinders();
        if(depDate == null) return;
        foreach (var d in depDate)
        {
            AddDependedCylinder(d);
        }
    }

    private DependedCylinder AddDependedCylinder(Saver.DependedLogicalCylinder dependedLogicalCylinder)
    {
        var obj = Instantiate(_dependedCylinder, transform);
        _cylinders.Add(obj);
        obj.SetFormula(dependedLogicalCylinder.Formula);
        obj.Name = dependedLogicalCylinder.Name;
        obj.SetMass(dependedLogicalCylinder.Mass);
        Resize();
        return obj;
    }

    public void AddNewCylinder()
    {
        var obj = Instantiate(_cylinder, transform);
        _cylinders.Add(obj);
        var pos = obj.transform.localPosition;
        pos.z = 0;
        obj.transform.localPosition = pos;
        obj.Name = $"g[{_cylinders.Count}]";
        Resize();
    }
    public Cylinder AddNewCylinder(Saver.LogicalCylinder logicalCylinder)
    {
        var obj = Instantiate(_cylinder, transform);
        _cylinders.Add(obj);
        var pos = obj.transform.localPosition;
        pos.z = logicalCylinder.Position;
        obj.SetMass(logicalCylinder.Mass);
        obj.transform.localPosition = pos;
        obj.Name = logicalCylinder.Name;
        Resize();
        return obj;
    }

    public void AddDependedCylinder()
    {
        var obj = Instantiate(_dependedCylinder, transform);
        _cylinders.Add(obj);
        obj.SetFormula("0");
        obj.Name = $"w[{_cylinders.Count}]";
        Resize();
    }

    public void Save()
    {
        List<Saver.LogicalCylinder> saveData = new List<Saver.LogicalCylinder>();
        List<Saver.DependedLogicalCylinder> saveData2 = new List<Saver.DependedLogicalCylinder>();
        foreach(var c in _cylinders)
        {
            if (c is DependedCylinder)
            {
                var sav = new Saver.DependedLogicalCylinder
                {
                    Position = c.transform.localPosition.z,
                    Name = c.Name,
                    Formula = (c as DependedCylinder).GetFormula(),
                    Mass = c.GetMass()
                };
                saveData2.Add(sav);
                continue;
            }
            var lCyl = new Saver.LogicalCylinder
            {
                Position = c.transform.localPosition.z,
                Name = c.Name,
                Mass = c.GetMass()
            };
            saveData.Add(lCyl);
        }
        Saver.Save(saveData);
        Saver.Save(saveData2);
    }
    
        [ContextMenu("Random")]
    private void PlaceRandomCylinders()
    {
        Clear();
        var count = Random.Range(2, 10);
        for (var i = 0; i < count; i+=2)
        {
            AddNewCylinder();
            AddDependedCylinder();
        }
        Resize();
    }
    
    [ContextMenu("Clear")]
    private void Clear()
    {
        _cylinders.ForEach(it => Destroy(it.gameObject));
        _cylinders.Clear();
    }

    public float GetAngle()
    {
        var ang = transform.rotation.eulerAngles.x > 180
            ? transform.rotation.eulerAngles.x - 360
            : transform.rotation.eulerAngles.x;
        return (ang+ _maxAngleOffset) / (2 * _maxAngleOffset);
    } 
}
