using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.Events;

public class BatterySlotScript : MonoBehaviour
{
    //public XRBaseInteractor interactor;
    public UnityEvent OnCorrectBattery;
    public string requiredBatterySuffix;

    private string currentBatteryName = null;
    private bool batteryInserted = false;

    void OnTriggerEnter(Collider other)
    {
        if (batteryInserted) return; // Prevent inserting multiple batteries

        if (other.gameObject.CompareTag("Battery"))
        {
            string batteryName = other.gameObject.name;
            Debug.Log($"Battery detected: {batteryName}, checking for suffix {requiredBatterySuffix}");

            if (!string.IsNullOrEmpty(requiredBatterySuffix) && batteryName.EndsWith(requiredBatterySuffix))
            {
                Debug.Log("Correct battery inserted.");

                currentBatteryName = batteryName;
                batteryInserted = true;

                //if (interactor != null)
                //{
                //    interactor.allowSelect = false; // Lock interaction for this slot
                //}

                OnCorrectBattery?.Invoke();
            }
            else
            {
                Debug.Log("Incorrect battery. Ignoring.");
            }
            //this.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public void EnterSlot()
    {
        //if (other.gameObject.CompareTag("Battery"))
        //{
        //    this.GetComponent<MeshRenderer>().enabled = false;
        //}

    }

    public void ExitSlot()
    {
        //this.GetComponent<MeshRenderer>().enabled = true;
    }

    //void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Battery"))
    //    {
    //        this.GetComponent<MeshRenderer>().enabled = true;
    //    }
    //}
}
