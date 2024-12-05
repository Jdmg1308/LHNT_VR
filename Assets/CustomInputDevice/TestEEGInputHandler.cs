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

        if (eegDevice.leftButtonInput.isPressed)
        {
            Debug.Log("Left button pressed!");
        }

        if (eegDevice.rightButtonInput.isPressed)
        {
            Debug.Log("Right button pressed!");
        }

        if (eegDevice.upButtonInput.isPressed)
        {
            Debug.Log("Up button pressed!");
        }

        if (eegDevice.downButtonInput.isPressed)
        {
            Debug.Log("Down button pressed!");
        }
    }
}