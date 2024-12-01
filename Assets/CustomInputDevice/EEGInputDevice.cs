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
public class EEGInputDevice : InputDevice, IInputUpdateCallbackReceiver
{
    public ButtonControl forwardButtonInput {get; private set;}
    public ButtonControl backwardButtonInput {get; private set;}

    protected override void FinishSetup(){
        base.FinishSetup();
        forwardButtonInput = GetChildControl<ButtonControl>("forwardButtonInput");
        backwardButtonInput = GetChildControl<ButtonControl>("backwardButtonInput");

    }

    static EEGInputDevice(){
        InputSystem.RegisterLayout<EEGInputDevice>(
            matches: new InputDeviceMatcher()
                .WithInterface("EEGInputDevice")
        );

        if(!InputSystem.devices.Any(x => x is EEGInputDevice)){
            // InputSystem.AddDevice<EEGInputDevice>();
            InputSystem.AddDevice(new InputDeviceDescription{
                interfaceName = "EEGInputDevice",
                product = "LhntEegVrHeadset"
            }); 
        }
        
        
    }

    [RuntimeInitializeOnLoadMethod]
    public static void Initialize(){
    }

    public void OnUpdate()
    {
        // Poll here    
        int buttonState = 0;

        // Check if the "K" key is pressed for forward input
        if (Keyboard.current.kKey.isPressed){
            buttonState |= 1 << 0; // Set bit 0 for forwardButtonInput
        }

        // Check if the "L" key is pressed for backward input
        if (Keyboard.current.lKey.isPressed){
            buttonState |= 1 << 1; // Set bit 1 for backwardButtonInput
        }

        // Queue the state event with the updated button state
        InputSystem.QueueStateEvent(this, new EEGInputState { buttons = buttonState });

    }
}
