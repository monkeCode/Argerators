using UnityEngine;

public class Panel : MonoBehaviour
{

    [SerializeField][Range(-180,180)] private float _angle;
    [SerializeField] private GameObject _cylinder;
    [SerializeField] private Transform _plane;

    [ContextMenu("Angle")]
    private void UpdateAngle()
    {
        transform.rotation = Quaternion.Euler(_angle,0,0);
    }

    [ContextMenu("Random")]
    private void PlaceRandomCylinders()
    {
        var count = Random.Range(2, 10);
        for (var i = 0; i < count; i++)
        {
            var obj = Instantiate(_cylinder, transform);
            obj.transform.localPosition = new Vector3(i*2-count + 0.5f, 1,Random.Range(-4, 4));
        }

        _plane.localScale = new Vector3(count/5.0f, 1, 1);
    }
    
}
