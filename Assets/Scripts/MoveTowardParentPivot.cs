using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardParentPivot : MonoBehaviour
{
    public float distance;
    public float time;
    
    private float _startTime;

    private void Awake()
    {
        _startTime = time;
        distance = Vector3.Distance(transform.position, transform.parent.position);
    }

    private void Update()
    {
        if (time > 0)
        {
            transform.position = Vector3.Lerp(transform.position, transform.parent.position, 1 - (time / _startTime));
            time -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
