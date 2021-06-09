using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LvlInitializator : MonoBehaviour
{
    [SerializeField] protected ServiceLocator ServiceLocator;
    public Services Services => ServiceLocator.Services;
}
