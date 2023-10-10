using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private Quaternion iniRot;
    
    void Start()
    {
        iniRot = transform.rotation;
    }

    void LateUpdate()
    {
        transform.rotation = iniRot;
    }
}
