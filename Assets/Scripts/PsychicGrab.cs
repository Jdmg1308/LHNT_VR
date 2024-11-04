using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PsychicGrab : MonoBehaviour
{
    public Transform GazeInteractorTransform;  // The transform that will act as the parent (e.g., controller)
    // public InputActionProperty grabAction;  // Input action for grabbing (assign the trigger button)
    public float rayDistance = 30.0f;  // Distance of the raycast
    public float grabSpeed = 10f;
    public LayerMask interactableLayer;  // Layer to limit the raycast to specific objects

    private Transform grabbedObject = null;
    public Transform holdPosition;
    private Transform grabbedObjectParentBuffer;

    [SerializeField] private Material crushedMaterial;

    private Material originalMaterial = null;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && grabbedObject == null)
        {
            AttemptGrab();
        }
        else if (Input.GetKeyDown(KeyCode.W) && grabbedObject != null)
        {
            ReleaseObject();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            CrushObject();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetObject();
        }
    }

    void AttemptGrab()
    {
        // Cast a ray from the controller forward
        Ray ray = new Ray(GazeInteractorTransform.position, transform.forward);
        RaycastHit hit;

        // If the ray hits an object within the specified layer and distance
        if (Physics.Raycast(ray, out hit, rayDistance, interactableLayer))
        {
            GrabbedObject(hit.transform);
        }
    }

    void GrabbedObject(Transform obj)
    {
        // Make the hit object a child of the controller
        grabbedObject = obj;
        grabbedObjectParentBuffer = grabbedObject.parent;

        grabbedObject.SetParent(GazeInteractorTransform);

        // Bring Object Closer
        StartCoroutine(MoveSmoothly(grabbedObject, holdPosition.position));

        // Get original object values
        originalMaterial = grabbedObject.GetComponent<MeshRenderer>().material;
    }

    void ReleaseObject()
    {
        // Release the grabbed object by setting its parent to null
        grabbedObject.SetParent(null);
        grabbedObject = null;
    }

    IEnumerator MoveSmoothly(Transform obj, Vector3 destination)
    {
        Debug.Log("bruhh");
        while (Vector3.Distance(obj.position, destination) > 0.01f)
        {
            // Lerp towards the target position over time
            obj.position = Vector3.Lerp(obj.position, destination, grabSpeed * Time.deltaTime);
            yield return null; // Wait for the next frame
        }

        // Snap to the destination to avoid small positional errors
        obj.position = destination;
    }


    public void CrushObject()
    {
        grabbedObject.GetComponent<MeshRenderer>().material = crushedMaterial;
        grabbedObject.GetComponent<CrushableScript>().Crush();
    }

    public void ResetObject()
    {
        grabbedObject.GetComponent<MeshRenderer>().material = originalMaterial;
    }

    void OnDrawGizmos()
    {
        // Set the color for the gizmo line
        Gizmos.color = Color.green;

        // Draw the ray from the object's position in the forward direction up to the specified ray distance
        Gizmos.DrawLine(GazeInteractorTransform.position, GazeInteractorTransform.position + GazeInteractorTransform.forward * rayDistance);
    }
}
