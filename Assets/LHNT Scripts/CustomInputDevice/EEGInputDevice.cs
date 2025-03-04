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
    // Define buttons with bits 0-5 for the six inputs
    [InputControl(name = "forwardButtonInput", layout = "Button", bit = 0)]
    [InputControl(name = "backwardButtonInput", layout = "Button", bit = 1)]
    [InputControl(name = "leftButtonInput", layout = "Button", bit = 2)]
    [InputControl(name = "rightButtonInput", layout = "Button", bit = 3)]
    [InputControl(name = "upButtonInput", layout = "Button", bit = 4)]
    [InputControl(name = "downButtonInput", layout = "Button", bit = 5)]
    public int buttons;
}

[InputControlLayout(displayName = "EEG Input Device", stateType = typeof(EEGInputState))]
[InitializeOnLoad]
public class EEGInputDevice : InputDevice, IInputUpdateCallbackReceiver
{
    public ButtonControl forwardButtonInput { get; private set; }
    public ButtonControl backwardButtonInput { get; private set; }
    public ButtonControl leftButtonInput { get; private set; }
    public ButtonControl rightButtonInput { get; private set; }
    public ButtonControl upButtonInput { get; private set; }
    public ButtonControl downButtonInput { get; private set; }


    protected override void FinishSetup()
    {
        base.FinishSetup();

        forwardButtonInput = GetChildControl<ButtonControl>("forwardButtonInput");
        backwardButtonInput = GetChildControl<ButtonControl>("backwardButtonInput");
        leftButtonInput = GetChildControl<ButtonControl>("leftButtonInput");
        rightButtonInput = GetChildControl<ButtonControl>("rightButtonInput");
        upButtonInput = GetChildControl<ButtonControl>("upButtonInput");
        downButtonInput = GetChildControl<ButtonControl>("downButtonInput");

    }

    static EEGInputDevice()
    {
        InputSystem.RegisterLayout<EEGInputDevice>(
            matches: new InputDeviceMatcher()
                .WithInterface("EEGInputDevice")
        );

        if (!InputSystem.devices.Any(x => x is EEGInputDevice))
        {
            // InputSystem.AddDevice<EEGInputDevice>();
            InputSystem.AddDevice(new InputDeviceDescription
            {
                interfaceName = "EEGInputDevice",
                product = "LhntEegVrHeadset"
            });
        }


    }

    [RuntimeInitializeOnLoadMethod]
    public static void Initialize()
    {
    }

    public void OnUpdate()
    {
        // Get the button state from WebSocketClient
        int buttonState = 0;

        buttonState |= 1 << WebSocketClient.buttonState;

        // Queue the state event with the updated button state
        InputSystem.QueueStateEvent(this, new EEGInputState { buttons = buttonState});
    }
}