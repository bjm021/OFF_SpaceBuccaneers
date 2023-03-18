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
    [SerializeField] private float cooldown = 1f;
    [SerializeField] private Card[] cards;
    
    private GameObject _spawnableUnitIndicator;
    private GameObject _nonSpawnableUnitIndicator;
    
    private Camera _mainCamera;
    private int _selectedUnitIndex;
    
    private bool _onCooldown;

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
        
        _spawnableUnitIndicator = unitIndicator.transform.GetChild(0).gameObject;
        _nonSpawnableUnitIndicator = unitIndicator.transform.GetChild(1).gameObject;
    }
    
    public void OnClick(InputValue value)
    {
        if (value.isPressed && !_onCooldown)
        {
            if (_selectedUnitIndex == 0)
            {
                return;
            }
            
            var clickPosition = Pointer.current.position.ReadValue();
            var ray = _mainCamera.ScreenPointToRay(clickPosition);

            if (!Physics.Raycast(ray, out var hit, 111, clickableLayers)) return;
            
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("NavMesh"))
            {
                if (_selectedUnitIndex >= 7)
                {
                    // TODO - Spawn Ability checks
                    AbilityManager.Instance.SpawnAbility(hit.point, _selectedUnitIndex-1, player);
                }
                else
                {
                    if ((clickPosition.x < Screen.width * spawnArea && player == GameManager.Player.PlayerOne || clickPosition.x > Screen.width * (1 - spawnArea) && player == GameManager.Player.PlayerTwo) 
                        && UnitManager.Instance.SpawnUnit(hit.point, UnitManager.Instance.UnitClasses[_selectedUnitIndex-1], player))
                    {
                        StartCoroutine(SpawnUnitCooldown());
                    }
                }
                
                
                
                DeselectUnit(_selectedUnitIndex);
            }
        }
    }

    private void Update()
    {
        var mousePosition = Pointer.current.position.ReadValue();
        
        unitIndicator.transform.position = _mainCamera.ScreenToWorldPoint(mousePosition);
        unitIndicator.transform.position = new Vector3(unitIndicator.transform.position.x, 10, unitIndicator.transform.position.z);

        if (_selectedUnitIndex == 0) return;
        
        if (_selectedUnitIndex < 7) // Unit
        {

            if (mousePosition.x < Screen.width * spawnArea && player == GameManager.Player.PlayerOne || mousePosition.x > Screen.width * (1 - spawnArea) && player == GameManager.Player.PlayerTwo)
            {
                if (UnitManager.Instance.UnitClasses[_selectedUnitIndex - 1].Cost <= GameManager.Instance.GetResource(player, GameManager.ResourceType.Metal))
                {
                    if (!_onCooldown)
                    {
                        ChangeSpawnableIndicator(true);
                        return;
                    }
                }
            }
        }
        else // Ability
        {
            if (AbilityManager.Instance.AbilityClasses[_selectedUnitIndex - 7].Cost <= GameManager.Instance.GetResource(player, GameManager.ResourceType.Crystals))
            {
                ChangeSpawnableIndicator(true);
                return;
            }
        }

        ChangeSpawnableIndicator(false);
    }
    
    private IEnumerator SpawnUnitCooldown()
    {
        _onCooldown = true;
        float time = 0;
        while (time < cooldown)
        {
            time += Time.deltaTime;
            UIManager.Instance.UpdateCooldownImages(1 - (time / cooldown));
            yield return null;
        }
        _onCooldown = false;
    }

    private void ChangeSpawnableIndicator(bool spawnable)
    {
        _spawnableUnitIndicator.SetActive(spawnable);
        _nonSpawnableUnitIndicator.SetActive(!spawnable);
    }

    public void SetSelectedUnitIndex(int index)
    {
        if (_selectedUnitIndex != 0)
        {
            DeselectUnit(_selectedUnitIndex);
        }
        SelectUnit(index);
    }

    private void SelectUnit(int index)
    {
        _selectedUnitIndex = index;
        UIManager.Instance.ShowSpawnableAreaIndicator(spawnArea, player);
        
        foreach (Transform child in _spawnableUnitIndicator.transform)
        {
            child.gameObject.SetActive(false);
        }
        foreach (Transform child in _nonSpawnableUnitIndicator.transform)
        {
            child.gameObject.SetActive(false);
        }

        cards[index - 1].IsSelected = true;
        
        _spawnableUnitIndicator.transform.GetChild(index - 1).gameObject.SetActive(true);
        _nonSpawnableUnitIndicator.transform.GetChild(index - 1).gameObject.SetActive(true);
    }

    private void DeselectUnit(int index)
    {
        _selectedUnitIndex = 0;
        UIManager.Instance.ShowSpawnableAreaIndicator(spawnArea, player, false);
        
        foreach (Transform child in _spawnableUnitIndicator.transform)
        {
            child.gameObject.SetActive(false);
        }
        
        foreach (Transform child in _nonSpawnableUnitIndicator.transform)
        {
            child.gameObject.SetActive(false);
        }

        cards[index - 1].IsSelected = false;
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
}
