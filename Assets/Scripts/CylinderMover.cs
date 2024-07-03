using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderMover : MonoBehaviour
{
    private Camera _camera;
    private InputSystem _input;
    private GameObject _cylinder;
    private Vector3 _mousePos;
    [SerializeField] private LayerMask _cylinderLayer;
    [SerializeField] private LayerMask _depCylLayer;
    [SerializeField] private LayerMask _planeLayer;
    void Start()
    {
        _camera = GetComponent<Camera>();
        _input = new InputSystem();
        _input.Enable();
        _input.Input.CylinderHandle.performed += ctx => TakeCylinder();
        _input.Input.CylinderHandle.canceled += ctx => DropCylinder();
        _input.Camera.PointerMove.performed += ctx => MoveCylinder(ctx.ReadValue<Vector2>());
        _input.Input.OpenCylMenu.performed += context => OpenCylMenu();
        
    }

    private void OpenCylMenu()
    {
        var cyl = GetCylUnderMouse(_cylinderLayer | _depCylLayer );
        if(cyl != null)
            Menu.Instance.ShowMenu(cyl.GetComponent<Cylinder>());
    }
    
    private void DropCylinder() => _cylinder = null;

    private void TakeCylinder()
    {
        _cylinder = GetCylUnderMouse(_cylinderLayer);
    }

    private GameObject GetCylUnderMouse(LayerMask layerMask)
    {
        var ray = _camera.ScreenPointToRay(_mousePos);
        if (Physics.Raycast(ray, out var hit, 100, layerMask) && hit.collider.TryGetComponent(out Cylinder _))
        {
            return hit.collider.gameObject;
        }

        return null;
    }

    private void MoveCylinder(Vector2 readValue)
    {
        _mousePos = readValue;
        if(_cylinder == null) return;
            var ray = _camera.ScreenPointToRay(_mousePos);
            var plane = Panel.Instance;
            var normal = plane.transform.up;
            var diff_point = ray.origin - plane.transform.position;
            var proj = Vector3.Dot(diff_point, normal) * normal;
            var new_point = ray.origin - proj;
            var new_vector = ray.direction - Vector3.Dot(ray.direction, normal) * normal;

            var n = (_cylinder.transform.position.x - new_point.x) / new_vector.x;
            var z = new_vector.z * n + new_point.z;

            var position = _cylinder.transform.localPosition;
            var newPos = new Vector3(position.x, position.y,  Mathf.Clamp(z - plane.transform.position.z, -5, 5));
            _cylinder.transform.localPosition = newPos;

            // Debug.Log($"{new_point}, {new_vector}");
            // Debug.DrawRay(_plane.transform.position, new_vector*10, color:Color.blue);
            // Debug.DrawRay(_plane.transform.position, _plane.transform.up, color:Color.red);
            // Debug.DrawLine(new_point, _plane.transform.position, color:Color.green);

    }

}
