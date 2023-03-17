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
    [SerializeField] private float moveAmount;
    [SerializeField] private float moveTime;
    [SerializeField] private float selectedOffset;

    private bool _isSelected;
    public bool IsSelected
    {
        get
        {
            return _isSelected;
        }
        set
        {
            _isSelected = value;
            StopAllCoroutines();
            StartCoroutine(MoveSelectedCardCoroutine(_isSelected));
        }
    }

    private RectTransform _rectTransform;
    private float _startPositionY;
    
    private void Start()
    {
        costText.text = unitClass.Cost.ToString();
        _rectTransform = transform.GetChild(0) as RectTransform;
        _startPositionY = _rectTransform.anchoredPosition.y;
    }

    private void OnMouseEnter()
    {
        MoveCard(true);
    }
    
    private void OnMouseExit()
    {
        MoveCard(false);
    }
    
    private void MoveCard(bool moveUp)
    {
        StopAllCoroutines();
        
        if (moveUp)
        {
            StartCoroutine(MoveCardCoroutine(true));
        }
        else
        {
            StartCoroutine(MoveCardCoroutine(false));
        }
    }
    
    private IEnumerator MoveCardCoroutine(bool moveUp)
    {
        // TODO: Also scale card
        var startPosition = _rectTransform.anchoredPosition.y;
        var endPosition = moveUp ? _startPositionY + moveAmount : IsSelected ? _startPositionY + selectedOffset : _startPositionY;
        var t = 0f;

        while (t < moveTime)
        {
            t += Time.deltaTime;
            _rectTransform.anchoredPosition = new Vector2(_rectTransform.anchoredPosition.x, Mathf.Lerp(startPosition, endPosition, t / moveTime));
            yield return null;
        }
    }
    
    private IEnumerator MoveSelectedCardCoroutine(bool moveUp)
    {
        var startPosition = _rectTransform.anchoredPosition.y;
        var endPosition = moveUp ? _startPositionY + selectedOffset : _startPositionY;
        var t = 0f;
        
        while (t < moveTime)
        {
            t += Time.deltaTime;
            _rectTransform.anchoredPosition = new Vector2(_rectTransform.anchoredPosition.x, Mathf.Lerp(startPosition, endPosition, t / moveTime));
            yield return null;
        }
    }
}
