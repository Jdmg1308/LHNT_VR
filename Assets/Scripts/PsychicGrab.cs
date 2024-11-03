using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PsychicGrab : MonoBehaviour
{
    public Transform parentTransform;  // The transform that will act as the parent (e.g., controller)
    // public InputActionProperty grabAction;  // Input action for grabbing (assign the trigger button)
    public float rayDistance = 30.0f;  // Distance of the raycast
    public LayerMask interactableLayer;  // Layer to limit the raycast to specific objects

    private Transform grabbedObject = null;

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
        Ray ray = new Ray(transform.position, transform.forward);
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
        grabbedObject.SetParent(parentTransform);

        // Bring Object Closer
        grabbedObject.position = new Vector3(parentTransform.position.x, grabbedObject.position.y, grabbedObject.position.z); // X, Y, Z coordinates

        // Get original object values
        originalMaterial = grabbedObject.GetComponent<MeshRenderer>().material;
    }

    void ReleaseObject()
    {
        // Release the grabbed object by setting its parent to null
        grabbedObject.SetParent(null);
        grabbedObject = null;
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
}
