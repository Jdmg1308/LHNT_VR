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
    [InputControl(name = "forwardButtonInput", layout = "Button", bit = 1)]
    [InputControl(name = "backwardButtonInput", layout = "Button", bit = 2)]
    [InputControl(name = "leftButtonInput", layout = "Button", bit = 3)]
    [InputControl(name = "rightButtonInput", layout = "Button", bit = 4)]
    [InputControl(name = "upButtonInput", layout = "Button", bit = 5)]
    [InputControl(name = "downButtonInput", layout = "Button", bit = 6)]
    public int buttons;
}

[InputControlLayout(displayName = "EEG Input Device", stateType = typeof(EEGInputState))]
// [InitializeOnLoad]
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
        int buttonState_1 = 0;
        int buttonState_2 = 0;

        string received = WebSocketClient.buttonState;

        int received_1 = int.Parse(received[0].ToString());
        int received_2 = (received.Length > 1) ? int.Parse(received[1].ToString()) : 0;

        Debug.Log($"received_1: {received_1}");
        Debug.Log($"received_2: {received_2}");

        if (received_1 > 0)
        {
            buttonState_1 = received_1;
        }

        if (received_2 > 0)
        {
            buttonState_2 = received_2;
        }

        // Check if the "w" key is pressed for forward input 
        if (Keyboard.current.wKey.isPressed)
        {
            buttonState_1 |= 1 << 1; // Set bit 1 for forwardButtonInput (moving foward)
        }
        // Check if the "s" key is pressed for backward input
        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            buttonState_1 |= 1 << 2; // Set bit 2 for backwardButtonInput (grabbing)
        }
        // Check if the "a" key is pressed for left input
        if (Keyboard.current.aKey.isPressed)
        {
            buttonState_1 |= 1 << 3; // Set bit 3 for leftButtonInput (stop)
        }
        // Check if the "d" key is pressed for right input
        if (Keyboard.current.dKey.isPressed)
        {
            buttonState_1 |= 1 << 4; // Set bit 4 for rightButtonInput (activate)
        }
        // Check if the "i" key is pressed for up input
        if (Keyboard.current.iKey.isPressed)
        {
            buttonState_1 |= 1 << 5; // Set bit 5 for upButtonInput (teleport)
        }
        // Check if the "k" key is pressed for down input
        if (Keyboard.current.kKey.isPressed)
        {
            buttonState_1 |= 1 << 6; // Set bit 6 for downButtonInput
        }


        if (buttonState_1 == 1){ //move forward
            if(PlayerScript.instance != null){
                PlayerScript.instance.MoveForward();
            }
        }

        // Queue the state event with the updated button state
        Debug.Log($"QueueStateEvent_1: {buttonState_1}");
        InputSystem.QueueStateEvent(this, new EEGInputState { buttons = buttonState_1});
        Debug.Log($"QueueStateEvent_2: {buttonState_2}");
        InputSystem.QueueStateEvent(this, new EEGInputState { buttons = buttonState_2});

    }
}