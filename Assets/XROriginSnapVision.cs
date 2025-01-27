using UnityEngine;
using Unity.XR.CoreUtils; // For accessing the XR Origin

public class XROriginSnapVision : MonoBehaviour
{
    public XROrigin xrOrigin; // Reference to the XR Origin
    public Transform cameraTransform; // Reference to the XR Camera (can be assigned manually or found automatically)

    public float rotationAmount = 30f; // Small amount to rotate when snapping
    public float angleThreshold = 90f; // Deadzone angle for snapping
    public float snapCooldown = 0.1f; // Time delay between snaps

    private float lastSnapTime; // To keep track of the last snap time

    void Start()
    {
        if (xrOrigin == null)
        {
            xrOrigin = FindObjectOfType<XROrigin>();
        }

        if (cameraTransform == null && xrOrigin != null)
        {
            cameraTransform = xrOrigin.Camera.gameObject.transform;
        }

        if (cameraTransform == null)
        {
            Debug.LogError("Camera Transform is not set!");
        }
    }

    public void Update()
    {

        if (cameraTransform == null || xrOrigin == null)
        {
            Debug.LogError("Missing references to XR Origin or Camera Transform.");
            return;
        }

        //Only allow snapping after cooldown
        if (Time.time - lastSnapTime < snapCooldown)
        {
            return;
        }

        // Get the current camera direction
        Vector3 cameraForward = cameraTransform.forward;
        // Project onto the horizontal plane
        cameraForward.y = 0;
        cameraForward.Normalize();

        Vector3 xrOriginForward = xrOrigin.transform.forward;
        xrOriginForward.y = 0;
        xrOriginForward.Normalize();


        // Calculate angles to left and right
        float angle = Vector3.SignedAngle(xrOriginForward, cameraForward, Vector3.up);

        Debug.Log(angle);

        // Check if the camera is within the threshold to snap

        if (Mathf.Abs(angle) > angleThreshold)
        {
            // Snap in the correct direction
            float snapDirection = Mathf.Sign(angle); // Positive for right, negative for left
            Snap(snapDirection * rotationAmount); // Snap by a small fixed amount
            lastSnapTime = Time.time; // Update cooldown timer
        }
    }

    /// <summary>
    /// Snaps the XR Origin to face left or right based on the current camera orientation.
    /// </summary>
    /// <param name="snapToRight">True to snap to the right, false to snap to the left.</param>
    public void Snap(float angle)
    {
        // Rotate the XR Origin
        xrOrigin.transform.Rotate(0, angle, 0);
        if (cameraTransform != null)
        {
            cameraTransform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            Debug.LogError("Camera Transform is null!");
        }

    }
}
