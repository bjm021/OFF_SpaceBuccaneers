using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
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
            var ray = _mainCamera.ScreenPointToRay(Pointer.current.position.ReadValue());
            if (!Physics.Raycast(ray, out var hit)) return;

            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("NavMesh")) // If player clicked on the game plane
            {
                if (_selectedUnitIndex == 0)
                {
                    // Do nothing
                }
                else
                {
                    Debug.Log($"Spawn unit {_selectedUnitIndex} at {hit.point}");
                    if (UnitManager.Instance.SpawnUnit(hit.point, UnitManager.Instance.UnitClasses[_selectedUnitIndex-1], Unit.UnitOwner.PlayerOne))
                    {
                        _selectedUnitIndex = 0;
                    }
                }
            }
            else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Unit")) // If player clicked on a unit
            {
                Debug.Log("Clicked on Unit {hit.transform.gameObject.name}}");
                // Show unit info
            }
            else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("UI")) // If player clicked on UI
            {
                // Do nothing
            }
        }
    }
    
    public void SetSelectedUnitIndex(int index)
    {
        _selectedUnitIndex = index;
        Debug.Log($"Selected unit index: {_selectedUnitIndex}");
    }
    
    public void OnEscape(InputValue value)
    {
        SetSelectedUnitIndex(0);
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
