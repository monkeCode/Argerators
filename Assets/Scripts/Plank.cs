
using UnityEngine;

public class Plank : MonoBehaviour
{
    [SerializeField] private Transform _followingTransform;
    public void Update()
    {
        transform.position = _followingTransform.position;
        transform.rotation = _followingTransform.rotation *Quaternion.Euler(new Vector3(90,0,0));
    }
}
