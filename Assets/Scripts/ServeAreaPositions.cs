using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServeAreaPositions : MonoBehaviour
{
    [SerializeField] Transform customerEntrance;
    [SerializeField] Transform customerServeAreaPosition;
    [SerializeField] Transform customerExit;

    public Transform CustomerEntrance
    {
        get { return customerEntrance; }
    }

    public Transform CustomerServeAreaPosition
    {
        get { return customerServeAreaPosition; }
    }

    public Transform CustomerExit
    {
        get { return customerExit; }
    }
}
