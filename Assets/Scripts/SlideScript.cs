using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlideScript : MonoBehaviour
{
    public InputActionAsset _playerControls;
    private InputAction _moveAction;
    public GameObject playerBody;
    // Start is called before the first frame updatepublic GameObject playerBody;

    void Awake()
    {
        _moveAction = _playerControls.FindActionMap("XRI Mind Locomotion").FindAction("Move");
    }

    void Update()
    {
        bool isMoving = _moveAction.ReadValue<float>() > .125f;
        if (isMoving){
            // Get the current position of the player body
            Vector3 currentPosition = playerBody.transform.position;

            // Calculate the new position based on the slide speed and direction
            Vector3 newPosition = currentPosition + (playerBody.transform.forward * Time.deltaTime * 5f); // Adjust speed as needed

            // Update the player's position
            playerBody.transform.position = newPosition;
        }
    } 
}
