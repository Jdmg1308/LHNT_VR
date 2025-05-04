Longhorn Neurotech VR Game compatible with 4 degrees of EEG Input


GitHub Repository: Link
Demo Video: Video (tutorial scene, not full gameplay)
Overview
This is a sci-fi VR game built in Unity that replaces traditional VR controller input with real-time EEG signals, allowing players to interact with the game world using brainwave-driven commands. The game is compatible with up to 4 degrees of EEG input, routed through a custom input pipeline.
EEG Input System
Why a custom input device?
 Unity’s Input System does not natively support EEG-based input. We implemented a custom InputDevice called EEGInputDevice to allow EEG data to be mapped to button inputs like forward, back, left, right, up, and down. This lets us use EEG input just like a keyboard, controller, or VR button — across any Unity component that supports the Input System.
Scripts Involved
* EEGInputDevice.cs
   * Registers a new input device type for EEG.
   * Maps EEG-based input into Unity’s input event queue using custom bitfields.
   * Can fall back on keyboard input for testing.
   * Handles device updates through OnUpdate() by checking WebSocketClient.buttonState.

   * WebSocketClient.cs
   * Connects to the EEG processing pipeline (e.g., Python script running on localhost or remote IP).
   * Receives JSON-formatted EEG vector data or raw numerical commands.
   * Parses and updates a global buttonState variable shared with the EEGInputDevice.
Robot AI
   * RobotAIScript.cs: Handles enemy patrols and attacks using a vision cone.
   * PatrolPathScript.cs: Assigns a patrol route to each robot.
   * DetectionAreaScript.cs: Triggers attack behavior when the player is within view.
Robots can be deactivated by player action and avoided using stealth and gaze-based interaction.
Player Input & Movement
Instead of hand controllers, input is controlled via:
   * XR Origin (in scene-hierarchy) is custom-made so it casts a “gaze” ray-cast (Mind Interactor) in the direction you are looking
   * Processes EEG input through interactions with this raycast rather than normal VR controller hands
   * Handles common VR actions like “activate”, “select”, “teleport”
   * XROriginSnapVision.cs: Snaps player's body rotation to match head direction, it  allows player to automatically turn their body when they turn their head
   * SlideScript.cs: Enables walking in gaze direction (based on EEG signal for "forward").
   * GameManager.cs and player UI allow for level transitions and player death to be notified and handled properly (restart game)
Gameplay Mechanics
Level Elements
   * Consists of a player navigating sci-fi environments looking to open doors by placing batteries into slots.
   * Can open the correct locked doors by picking up specifically colored Batteries and inserting them into the correct BatterySlots
   * BatterySlotScript.cs
   * Batteries belong to specific doors
   * Player evades the vision of patrolling Robots
   * Can defend themself by blocking projectiles with the battery item
Controls
   * Forward (W) - walk in the direction you are facing
   * Right (D) - activate (trigger)
   * Backward (S) - select (grip)
   * Up (I) - teleport (press to aim, release to teleport towards gaze)
   * Left - tbd (A as of now)
   * Down - tbd (K as of now)


Overview: EEG Input Architecture
The EEG integration for the Longhorn Neurotech VR Game follows this flow:
EEG Device → EEG Signal Processing Server → WebSocket → Unity Client


   1. EEG Device collects brainwave data in real time.
   2. EEG Signal Processing Server (Python-based, usually running on a laptop) analyzes and classifies this data into control signals.
   3. WebSocket Server transmits the control signal to Unity.
   4. Unity Client uses a custom input device (EEGInputDevice.cs) to convert WebSocket signals into in-game actions.
Setup Instructions
University Wi-Fi networks typically block WebSocket connections. You must use the same personal Wi-Fi or mobile hotspot for both server and Unity client.
Step-by-Step Setup
   1. Network Setup
   * Ensure both the EEG processing server (usually running on a laptop) and the Unity client (also a laptop or PC) are connected to the same Wi-Fi network or mobile hotspot.
   * Public/University networks often block necessary WebSocket ports.
   2. Open Unity Project
   * Inside the Unity scene hierarchy, locate the GameObject named EEG_device_websocket_client.
   * This GameObject contains the WebSocketClient.cs component.
   3. Set the IP Address
   * In the Inspector for EEG_device_websocket_client, you will see a text field labeled IP_address.
   * Set this to the IP address of the laptop running the EEG WebSocket server (e.g., 192.168.43.22).
   * Recommended: Use the text field via the Unity Editor.
   * Alternative: Hardcode the IP directly into WebSocketClient.cs
   4. Play the Game
   * Click the Play button in Unity.
   * Unity will attempt to connect to the WebSocket server using the provided IP.
   * If the connection fails:
   * A WebSocket error message will appear in the Console.
   * Unity will automatically pause the game.
   * You can unpause manually and continue testing the game using keyboard controls instead of EEG input.
Debugging & Fallback
   * If EEG server connection is not found:
   * Check that both machines are on the same subnet (i.e., IPs like 192.168.X.X).
   * Make sure the EEG server is running before Unity starts Play Mode.
   * Use the Console to view incoming WebSocket messages and connection status.
Assets used


We used the following assets


   * Droll Robot 03
   * Sci-Fi Styled Modular Pack
