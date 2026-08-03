using System;
using UnityEngine;

public class CameraUIOrbit : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Vector3 targetOffset; // Choose the part of the target to follow
    [SerializeField] private float distance; // Distance between the UI and Target
    
    private Camera mainCamera;
    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        Vector3 targetPoint = targetTransform.position + targetOffset;
        Vector3 cameraPoint = mainCamera.transform.right * distance;
        
        Vector3 finalPos = targetPoint + cameraPoint;
        transform.position = finalPos;
    }
}
