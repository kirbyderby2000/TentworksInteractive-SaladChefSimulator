using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Camera))]
public class MainCamera : MonoBehaviour
{
    public static Camera CameraInstance
    {
        private set;
        get;
    } = null; 

    private void Awake()
    {
        if(CameraInstance == null)
        {
            CameraInstance = GetComponent<Camera>();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

}
