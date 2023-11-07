using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimateHand : MonoBehaviour
{
    public InputActionProperty _pinchAnimation;
    public InputActionProperty _gripAnimation;
    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    
    void Update()
    {
        float triggerValue = _pinchAnimation.action.ReadValue<float>(); //Lo que va a hacer es almacenar la posicion del gatillo, esta en float
        _animator.SetFloat("Trigger", triggerValue);
        float gripValue = _gripAnimation.action.ReadValue<float>();
        _animator.SetFloat("Grip", gripValue);
        print(triggerValue);
    }
}
