using UnityEngine;

public class MovingPlatformManager : MonoBehaviour
{
    [SerializeField] private Transform platform;
    [SerializeField] private CharacterController playerController; // IMPORTANT: Move to GameManager when in real use.
    [SerializeField] private bool debugMode;
    
    private bool isUnderPlatform;
    private float lastTriggerStayTime;

    private void Awake()
    {
        if (platform == null)
            platform = transform;
    }

    private void OnTriggerStay(Collider other)
    {
        // Check the Character Controller root, not possibly untagged child colliders.
        if (playerController == null || !other.CompareTag("Player"))
            return;

        lastTriggerStayTime = Time.fixedTime;
        if (debugMode)
            Debug.DrawLine(platform.transform.position, other.transform.position, Color.blue, 0.1f);

        // Prevent SetParent from running every physics update.
        if (isUnderPlatform)
            return;

        // True preserves the player's current world position and rotation.
        playerController.transform.SetParent(platform, true);

        isUnderPlatform = true;
    }

    private void FixedUpdate()
    {
        if (!isUnderPlatform)
            return;

        // OnTriggerStay has stopped, so the player left the sensor.
        if (Time.fixedTime - lastTriggerStayTime > Time.fixedDeltaTime * 1.5f)
        {
            DetachPlayer();
        }
    }

    private void DetachPlayer()
    {
        if (playerController == null)
            return;

        playerController.transform.SetParent(null, true);

        isUnderPlatform = false;
    }

    private void OnDisable()
    {
        DetachPlayer();
    }
}