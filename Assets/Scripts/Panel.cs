using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour
{

    [SerializeField][Range(-180,180)] private float _angle;
    [SerializeField] private Cylinder _cylinder;
    [SerializeField] private DependedCylinder _dependedCylinder;
    [SerializeField] private Transform _plane;

    private List<Cylinder> _cylinders = new();
    
    [ContextMenu("Angle")]
    private void UpdateAngle()
    {
        transform.rotation = Quaternion.Euler(_angle,0,0);
    }

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

    public Cylinder AddNewCylinder()
    {
        var obj = Instantiate(_cylinder, transform);
        _cylinders.Add(obj);
        Resize();
        return obj;
    }

    public DependedCylinder AddDependedCylinder(Cylinder based)
    {
        var index = _cylinders.IndexOf(based);
        if(index == -1) return null;
        var obj = Instantiate(_dependedCylinder, transform);
        based.Add(obj);
        _cylinders.Insert(index,obj);
        Resize();
        return obj;
    }
    
        [ContextMenu("Random")]
    private void PlaceRandomCylinders()
    {
        Clear();
        var count = Random.Range(2, 10);
        for (var i = 0; i < count; i+=2)
        {
            var obj = Instantiate(_cylinder, transform);
            _cylinders.Add(obj);
            var depend = Instantiate(_dependedCylinder, transform);
            obj.Add(depend);
            _cylinders.Add(depend);
        }
        Resize();
    }
    
    [ContextMenu("Clear")]
    private void Clear()
    {
        _cylinders.ForEach(it => Destroy(it.gameObject));
        _cylinders.Clear();
    }
}
