using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(100)]
public class ObjectFollow : MonoBehaviour
{
    [SerializeField] private List<Transform> followPoints;
    [SerializeField] private float speed = 5.5f;
    [SerializeField] private bool debugMode;

    [Header("Waiting")]
    [SerializeField] private float waitTime = 2f;

    private int currentPoint;
    private float waitTimer;

    private void FixedUpdate()
    {
        if (followPoints == null || followPoints.Count == 0)
            return;

        if (currentPoint >= followPoints.Count)
            currentPoint = 0;

        Transform targetPoint = followPoints[currentPoint];

        if ((transform.position - targetPoint.position).sqrMagnitude > 0.000001f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPoint.position,
                speed * Time.fixedDeltaTime
            );
            
            if (debugMode)
                Debug.DrawLine(transform.position, targetPoint.position, Color.magenta, 0.1f);
            
            return;
        }

        waitTimer += Time.fixedDeltaTime;

        if (waitTimer < waitTime)
            return;

        waitTimer = 0f;
        currentPoint++;
    }
}