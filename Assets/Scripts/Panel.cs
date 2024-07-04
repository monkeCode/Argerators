using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Panel : MonoBehaviour
{

    [SerializeField] private Cylinder _cylinder;
    [SerializeField] private DependedCylinder _dependedCylinder;

    [SerializeField] private Transform _plane;
    [SerializeField] private float _maxAngleOffset = 23.20f;

    private float _angle;
    private List<Cylinder> _cylinders = new();

    public static Panel Instance { get; private set; }

    private void Update()
    {
        var depCyls = _cylinders.OfType<DependedCylinder>().ToList();
        var dict = GetCylPositions();
        foreach (var c in depCyls)
        {
            c.ChangePosition(dict);
        }
        
        _angle = CalculateAngle();
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(_angle* _maxAngleOffset,0,0),0.1f);
    }

    private float CalculateAngle()
    {
        var angle = 0.0f;
        foreach (var cyl in _cylinders)
        {
            float dist = (float)cyl.GetPos()*2-1f;
            angle += (dist) * cyl.GetMass();
        }
        //return angle / (_cylinders.Count>0?_cylinders.Count:1);
        return Mathf.Clamp(angle, -1, 1);
    }

    private Dictionary<string, double> GetCylPositions()=> 
        _cylinders.ToDictionary(cylinder => cylinder.name, cylinder=> cylinder.GetPos());

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
        Instance = this;
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
        obj.name = dependedLogicalCylinder.Name;
        obj.SetMass(dependedLogicalCylinder.Mass);
        Resize();
        return obj;
    }

    public void AddNewCylinder()
    {
        //TODO: needs normal name generation
        var obj = Instantiate(_cylinder, transform);
        _cylinders.Add(obj);
        obj.SetPos(0.5);
        obj.name = $"g[{_cylinders.Count}]";
        Resize();
    }
    public Cylinder AddNewCylinder(Saver.LogicalCylinder logicalCylinder)
    {
        var obj = Instantiate(_cylinder, transform);
        _cylinders.Add(obj);
        //var pos = obj.transform.localPosition;
        //pos.z = logicalCylinder.Position;
        obj.SetMass(logicalCylinder.Mass);
        obj.SetPos(logicalCylinder.Position);
        //obj.transform.localPosition = pos;
        obj.name = logicalCylinder.Name;
        Resize();
        return obj;
    }

    public void AddDependedCylinder()
    {
        var obj = Instantiate(_dependedCylinder, transform);
        _cylinders.Add(obj);
        obj.SetFormula("0");
        obj.name = $"w[{_cylinders.Count}]";
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
                    Position = c.GetPos(),
                    Name = c.name,
                    Formula = (c as DependedCylinder).GetFormula(),
                    Mass = c.GetMass()
                };
                saveData2.Add(sav);
                continue;
            }
            var lCyl = new Saver.LogicalCylinder
            {
                Position = c.GetPos(),
                Name = c.name,
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

    public void DeleteCyl(Cylinder cylinder)
    {
        _cylinders.Remove(cylinder);
        Destroy(cylinder.gameObject);
        Resize();
    }

    public float GetAngle()
    {
        return (_angle+1)/2;
    }

    public IReadOnlyList<Cylinder> GetCylinders() => _cylinders;
}
