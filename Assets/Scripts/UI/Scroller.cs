using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scroller : MonoBehaviour
{
    [SerializeField] private RawImage _img;
    [SerializeField] private float _x;
    private Vector2 _initialPosition;

    void Start()
    {
        _initialPosition = _img.uvRect.position;
        StartCoroutine(ResetPosition());
    }

    void Update()
    {
        _img.uvRect = new Rect(_img.uvRect.position + new Vector2(_x, 0) * Time.deltaTime, _img.uvRect.size);
    }

    IEnumerator ResetPosition()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.4f);
            _img.uvRect = new Rect(_initialPosition, _img.uvRect.size);
        }
    }
}