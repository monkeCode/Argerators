using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform _focusObject;

    [SerializeField] [Range(30, 50)] private float _minDistance;
    [SerializeField] [Range(60, 100)] private float _maxDistance;
    
    private InputSystem _input;
    private bool _isMoving;
    private Vector2 _startMousePos = Vector2.zero;
    private Vector2 _currentMousePos = Vector2.zero;

    private Vector3 _lookVector => (_focusObject.position - transform.position).normalized;
    
    private void Start()
    {
        _input = new InputSystem();
        _input.Enable();
        _input.Camera.Zoom.performed += ctx => Zoom(ctx.ReadValue<float>());
        _input.Camera.MoveState.performed += ctx => ChangeState(true);
        _input.Camera.MoveState.canceled += ctx => ChangeState(false);
        _input.Camera.PointerMove.performed += ctx => _currentMousePos = ctx.ReadValue<Vector2>();

    }

    private void Zoom(float dir)
    {
        transform.position += _lookVector * dir * Time.deltaTime;
    }

    private void StayInBounds()
    {
        var distance = Vector3.Distance(transform.position, _focusObject.position);
        if (distance >= _maxDistance)
            transform.position += _lookVector * (distance - _maxDistance);
        if (distance <= _minDistance)
            transform.position += _lookVector * (distance - _minDistance);
    }
    
    private void LookAt()
    {
        transform.LookAt(_focusObject);
    }

    private void ChangeState(bool state)
    {
        _isMoving = state;
        _startMousePos = _currentMousePos;
    }
    
    private void Move()
    {
        var dir = _currentMousePos - _startMousePos;
        transform.RotateAround(_focusObject.position,new Vector3(0,1,0), dir.x/300);
        transform.position =
            Vector3.Lerp(transform.position, 
                transform.position - transform.up * dir.y/100 * Vector3.Distance(_focusObject.position,transform.position), 
                Time.deltaTime);
    }

    private void Update()
    {
        StayInBounds();
        LookAt();
        if(_isMoving)
            Move();
    }
}
