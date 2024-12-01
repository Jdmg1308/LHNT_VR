using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestEEGInputHandler : MonoBehaviour
{
    private EEGInputDevice eegDevice;

    void Start()
    {
        // Find the EEGInputDevice in the active devices list
        eegDevice = InputSystem.devices.FirstOrDefault(d => d is EEGInputDevice) as EEGInputDevice;

        if (eegDevice == null)
        {
            Debug.LogError("EEG Input Device not found!");
        }
    }

    void Update()
    {
        if (eegDevice == null) return;

        if (eegDevice.forwardButtonInput.isPressed){
            Debug.Log("Forward button pressed!");
        }

        if (eegDevice.backwardButtonInput.isPressed){
            Debug.Log("Backward button pressed!");
            
        }
    }
}