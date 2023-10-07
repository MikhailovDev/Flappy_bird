using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchListener : MonoBehaviour
{
    public static event Action OnScreenTouched;

    private void OnMouseDown()
    {
        OnScreenTouched?.Invoke();
    }
}
