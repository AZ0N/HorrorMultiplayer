using UnityEngine;
using Unity.Netcode;
using System;
using UnityEngine.InputSystem;

public class PlayerNetwork : NetworkBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float mouseSensitivity = 30f;

    [Header("Camera & Audio")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private AudioListener audioListener;

    [Header("Flashlight")]
    [SerializeField] private Light flashlight;

    private float rotationX = 0;
    private float rotationY = 0;

    private InputActions inputActions;
    private InputActions.PlayerActions playerActions;

    private void Start()
    {
        if (!IsOwner) return;

        playerCamera.enabled = true;
        audioListener.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Setup input
        inputActions = new InputActions();
        playerActions = inputActions.Player;

        playerActions.Flashlight.performed += toggleFlashlight;
        playerActions.Focus.performed += focus;
        playerActions.Unfocus.performed += unFocus;

        inputActions.Player.Enable();
    }

    private Action<InputAction.CallbackContext> toggleFlashlight()
    {
        throw new NotImplementedException();
    }

    private void Update()
    {
        if (!IsOwner) return;

        // Only continue of the windows is focused
        if (Cursor.lockState != CursorLockMode.Locked) return;

        // Movement
        Vector2 moveDir = playerActions.Move.ReadValue<Vector2>();
        moveDir = moveDir.normalized;
        transform.position += (transform.forward * moveDir.y + transform.right * moveDir.x) * moveSpeed * Time.deltaTime;
        
        // Rotation/Looking
        Vector2 lookInput = playerActions.Look.ReadValue<Vector2>();
        lookInput *= Time.deltaTime * mouseSensitivity;
        
        rotationX -= lookInput.y;
        rotationY += lookInput.x;

        rotationX = Mathf.Clamp(rotationX, -90f, 90f);  // Clamp the X rotation to prevent the camera from flipping
        
        // Apply the rotation to the player and camera 
        transform.localRotation = Quaternion.Euler(0, rotationY, 0);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
    }

    private void toggleFlashlight(InputAction.CallbackContext context)
    {
        flashlight.gameObject.SetActive(!flashlight.gameObject.activeSelf);
    }

    private void focus(InputAction.CallbackContext context)
    {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
    }

    private void unFocus(InputAction.CallbackContext context)
    {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
    }
}