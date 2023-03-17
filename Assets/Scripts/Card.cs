using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] private UnitClass unitClass;
    [SerializeField] private TMP_Text costText;

    private void Start()
    {
        costText.text = unitClass.Cost.ToString();
    }

    private void OnMouseEnter()
    {
        throw new NotImplementedException();
    }
    
    private void OnMouseExit()
    {
        throw new NotImplementedException();
    }
}
