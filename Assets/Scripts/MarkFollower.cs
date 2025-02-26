using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MarkFollower : MonoBehaviour
{
    [SerializeField] private Transform _mark;
    [SerializeField] private Vector3 _offset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(_mark.transform.position.x - 1, transform.position.y, transform.position.z) + _offset;
    }
}
