using System;
using UnityEngine;

public class CameraUIBillboard : MonoBehaviour
{
    private Camera cam;
    
    private void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        transform.LookAt(cam.transform);
    }
}
