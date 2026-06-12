using System;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    [Tooltip("Where the raycast will be cast from. If left empty, it will be cast from the GameObject this script is attached to.")]
    [Header("Ground Detection Settings")]
    [SerializeField] private GameObject checkLocation;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float rayLength = 0.2f;
    [SerializeField] private float slopeAngleThreshold = 5f;
    [SerializeField] private bool useExtendedRangeOnSlope = true;
    [Header("Sphere Check Settings")]
    [SerializeField] private bool useSphereCheck;
    [SerializeField] private float sphereRadius = 0.3f;
    [SerializeField] private float sphereCastStartOffset = 0.2f;
    [Header("Debug Settings")]
    [SerializeField] private bool useDebugRays = true;
    
    public bool isGrounded { get; private set; }
    public bool isOnSlope { get; private set; }
    public Vector3 groundNormal { get; private set; }

    private void Start()
    {
        if (checkLocation == null)
            checkLocation = gameObject;
        
    }

    private void Update()
    {
        CheckGround();
    }

    private void CheckGround()
    {
        float currentRayLength = rayLength;
        RaycastHit hit;
        
        if (useExtendedRangeOnSlope && isOnSlope)
            currentRayLength *= 2.5f;
        
        // Check if there is ground below the check location

        if (useSphereCheck)
        {
            Vector3 sphereOrigin = checkLocation.transform.position + Vector3.up * sphereCastStartOffset;
            if (!Physics.SphereCast(sphereOrigin, sphereRadius, Vector3.down,
                    out hit, currentRayLength, groundLayer))
            {
                // There is no ground
                isGrounded = false;
                isOnSlope = false;
                groundNormal = Vector3.up;
                if (useDebugRays)
                    Debug.DrawRay(checkLocation.transform.position, Vector3.down * currentRayLength, Color.red);
                return;
            }
        }
        else
        {
            if (!Physics.Raycast(checkLocation.transform.position, Vector3.down,
                    out hit, currentRayLength, groundLayer))
            {
                // There is no ground
                isGrounded = false;
                isOnSlope = false;
                groundNormal = Vector3.up;
                if (useDebugRays)
                    Debug.DrawRay(checkLocation.transform.position, Vector3.down * currentRayLength, Color.red);
                return;
            }
        }

        // There is ground
        isGrounded = true;
        groundNormal = hit.normal;
        
        // Check if the ground is a slope
        isOnSlope = Vector3.Angle(hit.normal, Vector3.up) > slopeAngleThreshold;
        
        if (useDebugRays)
            Debug.DrawRay(checkLocation.transform.position, Vector3.down * currentRayLength, 
                isOnSlope ? Color.yellow : Color.green);
        
    }
    
    private void OnDrawGizmosSelected()
    {
        if (checkLocation == null || !useDebugRays || !useSphereCheck)
            return;
        
        float currentRayLength = rayLength;
        
        if (useExtendedRangeOnSlope && isOnSlope)
            currentRayLength *= 2.5f;

        Vector3 sphereOrigin = checkLocation.transform.position + Vector3.up * sphereCastStartOffset;
        Vector3 sphereEnd = sphereOrigin + Vector3.down * (currentRayLength + sphereCastStartOffset);

        Gizmos.color = !isGrounded ? Color.red : isOnSlope ? Color.yellow : Color.green;

        Gizmos.DrawWireSphere(sphereOrigin, sphereRadius);
        Gizmos.DrawWireSphere(sphereEnd, sphereRadius);
        Gizmos.DrawLine(sphereOrigin, sphereEnd);
    }
}
