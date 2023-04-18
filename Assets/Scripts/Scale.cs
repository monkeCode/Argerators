using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Scale : MonoBehaviour
{
    [SerializeField] private Transform _textTransform;

    [SerializeField] private TextMeshProUGUI _showableText;

    [SerializeField] private Transform _mark;

    [SerializeField] private Panel _plane;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(_mark.transform.position.x-1, transform.position.y, transform.position.z);

        var angle = _plane.GetAngle();
        _showableText.text = angle.ToString();
        _textTransform.localPosition = new Vector3(_textTransform.localPosition.x, angle * 6 - 3, _textTransform.localPosition.z);
        
    }
}
