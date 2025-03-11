using System;
using UnityEngine;

public class ComponentOptColor : MonoBehaviour
{
    public static EventHandler<int> OnChangePart;
    public void SetNumCurrentChoice1()
    {
        OnChangePart?.Invoke(this, 0);
    }
    public void SetNumCurrentChoice2()
    {
        OnChangePart?.Invoke(this, 1);
    }
    public void SetNumCurrentChoice3()
    {
        OnChangePart?.Invoke(this, 2);
    }
}
