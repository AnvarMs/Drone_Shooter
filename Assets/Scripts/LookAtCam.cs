using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class LookAtCam : MonoBehaviour
{
    
    
    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.transform);
    }
}
