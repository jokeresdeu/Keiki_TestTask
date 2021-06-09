using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "ServiceLocator", menuName = "ServiceLocator/ServiceLocator", order = 0)]
public class ServiceLocator : ScriptableObject
{
    public Services Services { get; private set; }

    public bool Inited { get; private set; }
    public void Init()
    {
        if (Services != null)
        {
            return;
        }
        Services = new Services();
    }   
}
