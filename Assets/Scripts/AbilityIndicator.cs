using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityIndicator : MonoBehaviour
{
    [SerializeField] private AbilityClass abilityClass;
    [SerializeField] private bool isMineField;
    [SerializeField] private bool isEmp;
    [SerializeField] private bool isBlackHole;
    [SerializeField] private bool isLaser;
    
    private Camera _camera;
    private Vector3 _laserPosition;

    private void Awake()
    {
        if (isMineField)
        {
            transform.localScale = new Vector3(abilityClass.MineRadius * 2, 0.5f, abilityClass.MineRadius * 2);
        }
        else if (isEmp)
        {
            transform.localScale = new Vector3(abilityClass.EmpRange * 2, 0.5f, abilityClass.EmpRange * 2);
        }
        else if (isBlackHole)
        {
            transform.localScale = new Vector3(abilityClass.BlackHoleRange * 2, 0.5f, abilityClass.BlackHoleRange * 2);
        }
        else if (isLaser)
        {
            if (GameManager.Instance.Host)
            {
                _laserPosition = GameManager.Instance.PlayerOneMothership.transform.GetChild(0).position;
            }
            else
            {
                _laserPosition = GameManager.Instance.PlayerTwoMothership.transform.GetChild(0).position;
            }
            
            transform.localScale = new Vector3(abilityClass.SpaceLaserWidth, 10, 1000);
        }
        
        _camera = Camera.main;
    }

    private void Update()
    {
        if (isLaser)
        {
            var worldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            worldPosition.y = 0;
            
            transform.position = _laserPosition;
            transform.LookAt(worldPosition);
        }
    }
}
