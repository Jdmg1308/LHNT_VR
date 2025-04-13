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
    public string requiredBatterySuffix;
    private string currentBatteryName = null;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Battery"))
        {
            Debug.Log("on trigger enter snappable? " + snappable);
        }
        if (other.gameObject.CompareTag("Battery") && snappable)
        {
            Debug.Log("OnTriggerAfterIf");
            if (this.GetComponent<MeshRenderer>().enabled)
            {
                currentBatteryName = other.gameObject.name; // Store the battery name
                interactor.allowSelect = true;
                snappable = false;
            }
            Debug.Log("inside after if snappable? " + snappable);
        }
    }

    public void EnterSlot(){
        Debug.Log("Attempting to enter slot...");

        if (currentBatteryName == null)
        {
            Debug.Log("No battery detected.");
            return;
        }

        Debug.Log($"Checking if {currentBatteryName} ends with {requiredBatterySuffix}");

        if (!string.IsNullOrEmpty(requiredBatterySuffix) && currentBatteryName.EndsWith(requiredBatterySuffix))
        {

            interactor.allowSelect = false;
            this.GetComponent<MeshRenderer>().enabled = false;
            OnCorrectBattery?.Invoke(); // Trigger event
        }
        else
        {
            Debug.Log(" Incorrect battery. Slot remains inactive.");
        }
    }

    public void ExitSlot()
    {
        Debug.Log("ExitSlot");
        currentBatteryName = null; // Clear stored battery
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
