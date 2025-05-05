using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class SlideScript : MonoBehaviour
{
    public GameObject playerBody;
    public float moveSpeed = 1.5f;

    private bool isMovingForward = false;
    public InputActionReference forwardButtonInput;

    private Rigidbody rb;

    void Start()
    {
        rb = playerBody.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody missing on playerBody!");
        }
    }

    void Update()
    {
        CheckEEGInput();

    }
    void FixedUpdate()
    {
        if (isMovingForward)
        {
            Vector3 gazeDirection = Camera.main.transform.forward;
            gazeDirection.y = 0f; // flatten to horizontal movement
            gazeDirection.Normalize();

            Vector3 move = gazeDirection * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + move);
        }
    }

    void CheckEEGInput()
    {
        foreach (var device in InputSystem.devices)
        {
            if (device.name == "EEGInputDevice")
            {
                var forward = device.TryGetChildControl<ButtonControl>("forwardButtonInput");
                var backward = device.TryGetChildControl<ButtonControl>("backwardButtonInput");
                var left = device.TryGetChildControl<ButtonControl>("leftButtonInput");
                var right = device.TryGetChildControl<ButtonControl>("rightButtonInput");
                var up = device.TryGetChildControl<ButtonControl>("upButtonInput");
                var down = device.TryGetChildControl<ButtonControl>("downButtonInput");

                // If any non-forward button is pressed, cancel forward movement
                if ((backward != null && backward.isPressed) ||
                    (left != null && left.isPressed) ||
                    (right != null && right.isPressed) ||
                    (up != null && up.isPressed))
                {
                    Debug.Log("forwards canceled");
                    isMovingForward = false;
                    return;
                }

                // If forward is pressed, activate forward movement
                if (forward != null && forward.isPressed)
                {
                    Debug.Log("mobing mobing");
                    isMovingForward = true;
                }

                // Otherwise, no input -> keep whatever state we were in
            }
        }
    }

}
