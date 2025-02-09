using UnityEngine;
using Unity.XR.CoreUtils; // For accessing the XR Origin

public class XROriginSnapVision : MonoBehaviour
{
    public XROrigin xrOrigin; // Reference to the XR Origin
    public Transform cameraTransform; // Reference to the XR Camera (can be assigned manually or found automatically)

    public float snapAngle = 45f; // Angle to snap per step
    public float angleThreshold = 30f; // Angle threshold to trigger snapping
    public float snapCooldown = 0.2f; // Time delay between snaps

    private float lastSnapTime = 0f;
    private bool hasSnapped = false;

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

    void Update()
    {
        if (cameraTransform == null || xrOrigin == null)
        {
            Debug.LogError("Missing references to XR Origin or Camera Transform.");
            return;
        }

        if (Time.time - lastSnapTime < snapCooldown)
        {
            return;
        }

        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();

        Vector3 xrOriginForward = xrOrigin.transform.forward;
        xrOriginForward.y = 0;
        xrOriginForward.Normalize();

        float angle = Vector3.SignedAngle(xrOriginForward, cameraForward, Vector3.up);

        if (!hasSnapped && Mathf.Abs(angle) > angleThreshold)
        {
            float snapDirection = Mathf.Sign(angle);
            Snap(snapDirection * snapAngle);
            lastSnapTime = Time.time;
            hasSnapped = true;
        }
        else if (Mathf.Abs(angle) < angleThreshold / 2)
        {
            hasSnapped = false;
        }
    }

    void Snap(float angle)
    {
        xrOrigin.transform.Rotate(0, angle, 0);
        cameraTransform.localRotation = Quaternion.identity;
    }
}
