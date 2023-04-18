using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class Cylinder : MonoBehaviour, ICollection<DependedCylinder>
{
    [SerializeField] private List<DependedCylinder> _dependedCylinders;

    [SerializeField] protected float _mass = 1;

    private float _lastZ;

    protected virtual void Start()
    {
        _lastZ = transform.localPosition.z;
    }

    protected virtual void Update()
    {
        Notify();
    }

    protected void Notify()
    {
        if (_lastZ != transform.localPosition.z)
        {
            _lastZ = transform.localPosition.z;
            _dependedCylinders.ForEach(it => it.ChangePosition(_lastZ/5.0f));
        }
    }

    public IEnumerator<DependedCylinder> GetEnumerator() => _dependedCylinders.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() =>GetEnumerator();

    public void Add(DependedCylinder item) => _dependedCylinders.Add(item);

    public void Clear()
    {
        foreach (var cyl in _dependedCylinders)
        {
            Destroy(cyl);
        }
        _dependedCylinders.Clear();
    }

    public bool Contains(DependedCylinder item)
    {
        return _dependedCylinders.Contains(item);
    }

    public void CopyTo(DependedCylinder[] array, int arrayIndex)
    {
        _dependedCylinders.CopyTo(array,arrayIndex);
    }

    public bool Remove(DependedCylinder item)
    {
       return _dependedCylinders.Remove(item);
    }

    public float GetMass() => _mass;

    public int Count => _dependedCylinders.Count;
    public bool IsReadOnly => false;
}
