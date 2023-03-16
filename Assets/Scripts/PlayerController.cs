using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameManager.Player player;
    [SerializeField] [Range(0.1f, 0.5f)] private float spawnArea = 0.33f;
    [SerializeField] private LayerMask clickableLayers;
    
    private Camera _mainCamera;
    private int _selectedUnitIndex = 0;

    private void Awake()
    {
        _mainCamera = Camera.main;

        if (_mainCamera == null)
        {
            Debug.LogError("Main Camera not found");
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
                    Debug.Log($"Spawn unit {_selectedUnitIndex} at {hit.point}");
                    _selectedUnitIndex = 0;
                    UIManager.Instance.ShowSpawnableAreaIndicator(spawnArea, player, false);
                }
            }
        }
    }
    
    public void SetSelectedUnitIndex(int index)
    {
        if (index == _selectedUnitIndex)
        {
            _selectedUnitIndex = 0;
            UIManager.Instance.ShowSpawnableAreaIndicator(spawnArea, player, false);
            Debug.Log("Deselected Unit");
        }
        
        _selectedUnitIndex = index;
        UIManager.Instance.ShowSpawnableAreaIndicator(spawnArea, player);
        Debug.Log($"Selected unit index: {_selectedUnitIndex}");
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
