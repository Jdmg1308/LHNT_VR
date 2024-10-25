using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushableScript : MonoBehaviour
{
    [SerializeField] private Material crushedMaterial;
    private Material originalMaterial;

    void Start() {
        originalMaterial = GetComponent<MeshRenderer>().material;
    }
    
    public void Crush(){
        GetComponent<MeshRenderer>().material = crushedMaterial;
    }

    public void Reset(){
        GetComponent<MeshRenderer>().material = originalMaterial;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Crush();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reset();
        }        
    }
}
