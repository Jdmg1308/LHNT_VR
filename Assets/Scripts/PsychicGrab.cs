using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PsychicGrab: MonoBehaviour
{
    public Transform parentTransform;  // The transform that will act as the parent (e.g., controller)
    // public InputActionProperty grabAction;  // Input action for grabbing (assign the trigger button)
    public float rayDistance = 30.0f;  // Distance of the raycast
    public LayerMask interactableLayer;  // Layer to limit the raycast to specific objects

    private Transform grabbedObject = null;

    void Update()
    {
        // Check if the grab action (e.g., trigger button) is pressed
        //if (grabAction.action.WasPressedThisFrame())
        //{
        //    AttemptGrab();
        //}
        //else if (grabAction.action.WasReleasedThisFrame() && grabbedObject != null)
        //{
        //    ReleaseObject();
        //}
        if (Input.GetKeyDown(KeyCode.W) && grabbedObject == null)
        {
            AttemptGrab();
        }
        else if (Input.GetKeyDown(KeyCode.W) && grabbedObject != null)
        {
            ReleaseObject();
        }
    }

    void AttemptGrab()
    {
        // Cast a ray from the controller forward
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // If the ray hits an object within the specified layer and distance
        if (Physics.Raycast(ray, out hit, rayDistance, interactableLayer))
        {
            // Make the hit object a child of the controller
            grabbedObject = hit.transform;
            grabbedObject.SetParent(parentTransform);
        }
    }

    void ReleaseObject()
    {
        // Release the grabbed object by setting its parent to null
        grabbedObject.SetParent(null);
        grabbedObject = null;
    }
}
