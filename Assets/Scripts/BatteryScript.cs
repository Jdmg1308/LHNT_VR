using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class BatteryScript : MonoBehaviour
{
    bool grabbable = true;
    public XRBaseInteractor interactor;

    void OnTriggerEnter(Collider other){
        if (other.gameObject.CompareTag("BatterySlot") && grabbable){
            interactor.allowSelect = false;
            grabbable = false;
            Invoke("MakeGrabbable", 1f);
        }
    }

    void MakeGrabbable(){
        grabbable = true;
        // GetComponent<XRGrabInteractable>().
        interactor.allowSelect = true;
    }
}
