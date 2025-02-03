using UnityEngine;
using Unity.XR.CoreUtils; // For accessing the XR Origin

public class XROriginSnapVision : MonoBehaviour
{
    public XROrigin xrOrigin; // Reference to the XR Origin
    public Transform cameraTransform; // Reference to the XR Camera (can be assigned manually or found automatically)

    public float rotationSpeed = 90f; // Degrees per second
    public float angleThreshold = 90f; // Deadzone angle for starting rotation

    private bool isRotating = false;
    private float targetAngle = 0f;

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

        if (isRotating)
        {
            SmoothRotate();
            return;
        }

        // Get the current camera direction
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();

        Vector3 xrOriginForward = xrOrigin.transform.forward;
        xrOriginForward.y = 0;
        xrOriginForward.Normalize();

        float angle = Vector3.SignedAngle(xrOriginForward, cameraForward, Vector3.up);

        if (Mathf.Abs(angle) > angleThreshold)
        {
            float rotationDirection = Mathf.Sign(angle);
            targetAngle = xrOrigin.transform.eulerAngles.y + rotationDirection * angleThreshold;
            isRotating = true;
        }
    }

    void SmoothRotate()
    {
        float currentY = xrOrigin.transform.eulerAngles.y;
        float newY = Mathf.MoveTowardsAngle(currentY, targetAngle, rotationSpeed * Time.deltaTime);
        xrOrigin.transform.eulerAngles = new Vector3(0, newY, 0);

        if (Mathf.Approximately(newY, targetAngle))
        {
            isRotating = false;
        }
    }
}