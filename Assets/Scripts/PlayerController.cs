using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameManager.Player player;
    [SerializeField] [Range(0.1f, 0.5f)] private float spawnArea = 0.33f;
    [SerializeField] private LayerMask clickableLayers;
    [SerializeField] private GameObject unitIndicator;
    
    private Camera _mainCamera;
    private int _selectedUnitIndex = 0;

    private void Awake()
    {
        if (!GameManager.Instance.Host)
        {
            player = GameManager.Player.PlayerTwo;
        }
        
        _mainCamera = Camera.main;

        if (_mainCamera == null)
        {
            Debug.LogError("Main Camera not found");
        }
        
        if (player == GameManager.Player.PlayerOne)
        {
            unitIndicator.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else
        {
            unitIndicator.transform.rotation = Quaternion.Euler(0, -90, 0);
        }
    }
    
    public void OnClick(InputValue value)
    {
        if (value.isPressed)
        {
            var clickPosition = Pointer.current.position.ReadValue();
            var ray = _mainCamera.ScreenPointToRay(clickPosition);

            if (!Physics.Raycast(ray, out var hit, 111, clickableLayers)) return;

            if (_selectedUnitIndex == 0)
            {
                // Do nothing
            }
            else
            {
                if ((clickPosition.x < Screen.width * spawnArea && player == GameManager.Player.PlayerOne || clickPosition.x > Screen.width * (1 - spawnArea) && player == GameManager.Player.PlayerTwo) 
                    && UnitManager.Instance.SpawnUnit(hit.point, UnitManager.Instance.UnitClasses[_selectedUnitIndex-1], GameManager.Player.PlayerOne))
                {
                    DeselectUnit();
                }
            }
        }
    }

    private void Update()
    {
        unitIndicator.transform.position = _mainCamera.ScreenToWorldPoint(Pointer.current.position.ReadValue());
        unitIndicator.transform.position = new Vector3(unitIndicator.transform.position.x, 10, unitIndicator.transform.position.z);
    }

    public void SetSelectedUnitIndex(int index)
    {
        if (index == _selectedUnitIndex)
        {
            DeselectUnit();
        }
        
        SelectUnit(index);
    }

    private void SelectUnit(int index)
    {
        _selectedUnitIndex = index;
        UIManager.Instance.ShowSpawnableAreaIndicator(spawnArea, player);
        
        foreach (Transform child in unitIndicator.transform)
        {
            child.gameObject.SetActive(false);
        }
        
        unitIndicator.transform.GetChild(index - 1).gameObject.SetActive(true);
    }

    private void DeselectUnit()
    {
        _selectedUnitIndex = 0;
        UIManager.Instance.ShowSpawnableAreaIndicator(spawnArea, player, false);
        
        foreach (Transform child in unitIndicator.transform)
        {
            child.gameObject.SetActive(false);
        }
    }
    
    public void OnEscape(InputValue value)
    {
        UIManager.Instance.TogglePauseMenu();
    }
    
    public void OnSelectUnit1(InputValue value)
    {
        SetSelectedUnitIndex(1);
    }
    
    public void OnSelectUnit2(InputValue value)
    {
        SetSelectedUnitIndex(2);
    }
    
    public void OnSelectUnit3(InputValue value)
    {
        SetSelectedUnitIndex(3);
    }
    
    public void OnSelectUnit4(InputValue value)
    {
        SetSelectedUnitIndex(4);
    }
    
    public void OnSelectUnit5(InputValue value)
    {
        SetSelectedUnitIndex(5);
    }
    
    public void OnSelectUnit6(InputValue value)
    {
        SetSelectedUnitIndex(6);
    }
    
    public void OnSelectUnit7(InputValue value)
    {
        SetSelectedUnitIndex(7);
    }
    
    public void OnSelectUnit8(InputValue value)
    {
        SetSelectedUnitIndex(8);
    }
    
    public void OnSelectUnit9(InputValue value)
    {
        SetSelectedUnitIndex(9);
    }
    
    public void OnSelectUnit10(InputValue value)
    {
        SetSelectedUnitIndex(10);
    }
    
    public void OnSelectUnit11(InputValue value)
    {
        SetSelectedUnitIndex(11);
    }
    
    public void OnSelectUnit12(InputValue value)
    {
        SetSelectedUnitIndex(12);
    }
}
