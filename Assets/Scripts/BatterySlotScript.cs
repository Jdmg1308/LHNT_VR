using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.Events;

public class BatterySlotScript : MonoBehaviour
{
    public bool snappable = true;
    public XRBaseInteractor interactor;
    public UnityEvent OnCorrectBattery;

    void OnTriggerEnter(Collider other){
        if (other.gameObject.CompareTag("Battery"))
        {
            Debug.Log("on trigger enter snappable? " + snappable);
        }
        if (other.gameObject.CompareTag("Battery") && snappable)
        {
            Debug.Log("OnTriggerAfterIf");
            if (this.GetComponent<MeshRenderer>().enabled)
            {
                interactor.allowSelect = false;
                snappable = false;
            }
            Debug.Log("inside after if snappable? " + snappable);
        }
    }

    public void EnterSlot(){
        Debug.Log("EnterSlot");
        interactor.allowSelect = true;
        this.GetComponent<MeshRenderer>().enabled = false;
        OnCorrectBattery?.Invoke();
    }

    public void ExitSlot()
    {
        Debug.Log("ExitSlot");
        StartCoroutine(EnableSnappableWithDelay(0.5f));
        this.GetComponent<MeshRenderer>().enabled = true;
    }

    private IEnumerator EnableSnappableWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        snappable = true;
        Debug.Log("Snappable is now true after delay");
    }
}
