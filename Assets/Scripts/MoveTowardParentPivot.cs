using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardParentPivot : MonoBehaviour
{
    public float time;
    
    private Vector3 _startPosition;
    private float _startTime;

    private void Awake()
    {
        _startTime = time;
        _startPosition = transform.position;
    }

    private void Update()
    {
        if (time > 0)
        {
            transform.position = Vector3.Lerp(_startPosition, transform.parent.position, 1 - time / _startTime);
            time -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
