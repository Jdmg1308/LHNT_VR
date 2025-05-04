# Longhorn Neurotech VR Game

**Compatible with 4 Degrees of EEG Input**  
**GitHub Repository:** [\[Link\] ](https://github.com/Jdmg1308/LHNT_VR) 
**Demo Video:** [\[Video (tutorial scene, not full gameplay)\]](https://www.youtube.com/watch?v=Gs7hyJFAAt8)

---

## Overview

This is a sci-fi VR game built in Unity that replaces traditional VR controller input with real-time EEG signals, allowing players to interact with the game world using brainwave-driven commands. The game is compatible with up to 4 degrees of EEG input, routed through a custom input pipeline.

---

## EEG Input System

### Why a Custom Input Device?

Unity’s Input System does not natively support EEG-based input. We implemented a custom `InputDevice` called `EEGInputDevice` to allow EEG data to be mapped to button inputs like forward, back, left, right, up, and down. This lets us use EEG input just like a keyboard, controller, or VR button — across any Unity component that supports the Input System.

---

### Scripts Involved

#### EEGInputDevice.cs
- Registers a new input device type for EEG.
- Maps EEG-based input into Unity’s input event queue using custom bitfields.
- Can fall back on keyboard input for testing.
- Handles device updates through `OnUpdate()` by checking `WebSocketClient.buttonState`.

#### WebSocketClient.cs
- Connects to the EEG processing pipeline (e.g., Python script running on localhost or remote IP).
- Receives JSON-formatted EEG vector data or raw numerical commands.
- Parses and updates a global `buttonState` variable shared with the `EEGInputDevice`.

---

## Robot AI

- `RobotAIScript.cs`: Handles enemy patrols and attacks using a vision cone.
- `PatrolPathScript.cs`: Assigns a patrol route to each robot.
- `DetectionAreaScript.cs`: Triggers attack behavior when the player is within view.
- Robots can be deactivated by player action and avoided using stealth and gaze-based interaction.

---

## Player Input & Movement

- XR Origin (custom): casts a “gaze” raycast (Mind Interactor) in the direction you are looking.
- EEG input is used to interact with this raycast instead of normal VR controller hands.
- Supports actions like “activate”, “select”, “teleport”.

#### Movement Scripts
- `XROriginSnapVision.cs`: Snaps player body rotation to match head direction.
- `SlideScript.cs`: Enables walking in gaze direction (based on EEG signal for "forward").
- `GameManager.cs`: Handles level transitions, player death, and UI state.

---

## Gameplay Mechanics

### Level Elements
- Players explore sci-fi environments and open doors by placing batteries into slots.
- Batteries must match the correct color and door pairing.
- Robots patrol areas and must be evaded or blocked using batteries as shields.

#### BatterySlotScript.cs
- Batteries are linked to specific doors.
- Only the correct battery will unlock a specific door.

---

## Controls

- Forward (W): Walk in direction of gaze  
- Right (D): Activate (Trigger)  
- Backward (S): Select (Grip)  
- Up (I): Teleport (Aim)  
- Down (K): Teleport (Release)  
- Left (A): TBD  

---

## EEG Input Architecture

EEG Device → EEG Signal Processing Server → WebSocket → Unity Client


1. EEG Device: Collects brainwave data in real time.  
2. EEG Server (Python): Classifies the data into commands (e.g. "forward").  
3. WebSocket: Sends commands from server to Unity client.  
4. Unity Client: Converts EEG signals into in-game actions using `EEGInputDevice.cs`.  

---

## Setup Instructions

### Network Requirements

University Wi-Fi often blocks WebSocket connections. You must use a personal Wi-Fi or mobile hotspot where both server and Unity client are connected to the same network.

### Step-by-Step Setup

1. **Network Setup**  
   Ensure EEG server and Unity client are on the same local network or hotspot.  
   Avoid university/public networks.

2. **Open Unity Project**  
   In the Unity scene, find the GameObject: `EEG_device_websocket_client`.  
   This contains the `WebSocketClient.cs` script.

3. **Set the IP Address**  
   In the Inspector, set the `IP_address` field to the server laptop's IP (e.g. `192.168.43.22`).  
   - Recommended: Use the Unity Editor field.  
   - Alternative: Hardcode IP into `WebSocketClient.cs`.

4. **Play the Game**  
   Click **Play** in the Unity Editor.  
   Unity will attempt to connect to the EEG server.  
   If the connection fails:  
   - An error message will show in the Console.  
   - Unity auto-pauses the game.  
   - You can manually unpause and use keyboard fallback controls.

---

## Debugging & Fallback

- Confirm both devices are on the same subnet (e.g., `192.168.X.X`).  
- Start the EEG server first, then Unity.  
- Use Unity's Console to check connection messages.  
- Keyboard input works as fallback if EEG input is unavailable.

---

## Assets Used

- Droll Robot 03: https://assetstore.unity.com/packages/3d/characters/robots/droll-robot-03-245228
- Sci-Fi Styled Modular Pack: https://assetstore.unity.com/packages/3d/environments/sci-fi/sci-fi-styled-modular-pack-82913
