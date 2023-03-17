using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class EscapeManager : MonoBehaviour
{
    [SerializeField] private UnityEvent onEscape;

    private void OnEscape(InputValue value)
    { 
        onEscape.Invoke();
    }
}
