using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;

// Added to auto run static constructor after C# domain load
#if UNITY_EDITOR
[InitializeOnLoad]
#endif
[InputControlLayout(displayName = "EEG Input Device")]
public class EEGInputDevice : InputDevice
{
    public ButtonControl button1 {get; private set;}
    public ButtonControl button2 {get; private set;}
    public ButtonControl button3 {get; private set;}
    public ButtonControl button4 {get; private set;}
    public ButtonControl button5 {get; private set;}
    public ButtonControl button6 {get; private set;}

    static EEGInputDevice(){
        // Adds control layout to the the input system
        InputSystem.RegisterLayout<EEGInputDevice>();
    }

    // Apparently this triggers static constructor of our input device in the player
    [RuntimeInitializeOnLoadMethod]
    private static void InitializeInPlayer() {}

    // Input System call this after it constructs the device and before it adds the device to system
    // Any last setup can be added here
    protected override void FinishSetup(){
        base.FinishSetup();

        button1 = GetChildControl<ButtonControl>("button1");
        button2 = GetChildControl<ButtonControl>("button2");
        button3 = GetChildControl<ButtonControl>("button3");
        button4 = GetChildControl<ButtonControl>("button4");
        button5 = GetChildControl<ButtonControl>("button5");
        button6 = GetChildControl<ButtonControl>("button6");
    }

    [MenuItem("Tools/Add EEG Input Device")]
    public static void Initialize(){
        // This is called by the Input System to initialize the device
        InputSystem.AddDevice<EEGInputDevice>();
    }
}
