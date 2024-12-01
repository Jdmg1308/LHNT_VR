using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;


public struct EEGInputState : IInputStateTypeInfo
{
    public FourCC format => new FourCC('E', 'E', 'G', 'I');
    [InputControl(name = "forwardButtonInput", layout = "Button", bit = 0)]
    [InputControl(name = "backwardButtonInput", layout = "Button", bit = 1)]
    public int buttons;
}

[InputControlLayout(displayName = "EEG Input Device", stateType = typeof(EEGInputState))]
[InitializeOnLoad]
public class EEGInputDevice : InputDevice
{
    public ButtonControl forwardButtonInput {get; private set;}
    public ButtonControl backwardButtonInput {get; private set;}

    // static EEGInputDevice(){
    //     // Adds control layout to the the input system
    //     InputSystem.RegisterLayout<EEGInputDevice>();
    // }

    // Apparently this triggers static constructor of our input device in the player
    // [RuntimeInitializeOnLoadMethod]
    // private static void InitializeInPlayer() {}

    // Input System call this after it constructs the device and before it adds the device to system
    // Any last setup can be added here
    protected override void FinishSetup(){
        base.FinishSetup();
        forwardButtonInput = GetChildControl<ButtonControl>("forwardButtonInput");
        backwardButtonInput = GetChildControl<ButtonControl>("backwardButtonInput");
    //     // button3 = GetChildControl<ButtonControl>("button3");
    //     // button4 = GetChildControl<ButtonControl>("button4");
    //     // button5 = GetChildControl<ButtonControl>("button5");
    //     // button6 = GetChildControl<ButtonControl>("button6");
    }

    static EEGInputDevice(){
        InputSystem.RegisterLayout<EEGInputDevice>();

        if(!InputSystem.devices.Any(x => x is EEGInputDevice)){
            InputSystem.AddDevice<EEGInputDevice>();
        }
        
    }

    [RuntimeInitializeOnLoadMethod]
    public static void Initialize(){
        // This is called by the Input System to initialize the device
        // var eegInputDevice = InputSystem.AddDevice<EEGInputDevice>();
        // InputSystem.QueueStateEvent(eegInputDevice, new EEGInputState{buttons = 0});
        
    }
}
