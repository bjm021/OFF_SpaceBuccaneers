using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class WobbleAndRotate : MonoBehaviour
{
    [SerializeField] private bool3 wobbleAxis = new bool3(false, true, false);
    [SerializeField] private float wobbleSpeed = 1f;
    [SerializeField] private float wobbleAmount = 0.1f;
    [Space]
    [SerializeField] private bool3 rotationAxis = new bool3(false, true, false);
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private float rotationAmount = 10f;

    private Vector3 startPosition;
    private Quaternion startRotation;

    private void Awake()
    {
        startPosition = transform.localPosition;
        startRotation = transform.localRotation;
    }

    private void Update()
    {
        transform.localRotation = Quaternion.Euler(
            rotationAxis.x ? math.sin(Time.time * rotationSpeed) * rotationAmount : 0,
            rotationAxis.y ? math.sin(Time.time * rotationSpeed) * rotationAmount : 0,
            rotationAxis.z ? math.sin(Time.time * rotationSpeed) * rotationAmount : 0) * startRotation;

        transform.localPosition = startPosition + new Vector3(
            wobbleAxis.x ? math.sin(Time.time * wobbleSpeed) * wobbleAmount : 0,
            wobbleAxis.y ? math.sin(Time.time * wobbleSpeed) * wobbleAmount : 0,
            wobbleAxis.z ? math.sin(Time.time * wobbleSpeed) * wobbleAmount : 0);
    }
}
