using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderMover : MonoBehaviour
{
    private Camera _camera;
    private InputSystem _input;
    private GameObject _cylinder;
    private Vector3 _mousePos;
    private GameObject _plane;
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
            if (!Physics.Raycast(ray, out var hit, 100, _planeLayer)) return;
            var position = _cylinder.transform.position;

            var newPos = new Vector3(position.x, hit.point.y+ hit.normal.y, hit.point.z);
            _cylinder.transform.position = newPos;

    }

}
