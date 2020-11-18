using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component used to locate a salad bowl on a game object
/// </summary>
public class SaladLocator : MonoBehaviour
{
    [SerializeField] SaladBowl saladBowl;

    public SaladBowl SaladBowlAttached
    {
        get { return saladBowl; }
    }
}
