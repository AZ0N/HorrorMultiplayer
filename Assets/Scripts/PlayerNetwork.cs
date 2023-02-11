using UnityEngine;
using Unity.Netcode;

public class PlayerNetwork : NetworkBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float mouseSensitivity = 100f;

    [Header("Camera & Audio")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private AudioListener audioListener;

    [Header("Flashlight")]
    [SerializeField] private Light flashlight;

    private float rotationX = 0;
    private float rotationY = 0;
    private Vector2 moveDir;

    private void Start()
    {
        if (!IsOwner) return;

        playerCamera.enabled = true;
        audioListener.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (!IsOwner) return;

        moveDir = Vector2.zero;
        if (Input.GetKey(KeyCode.W)) moveDir.y += 1;
        if (Input.GetKey(KeyCode.S)) moveDir.y -= 1;
        if (Input.GetKey(KeyCode.A)) moveDir.x -= 1;
        if (Input.GetKey(KeyCode.D)) moveDir.x += 1;

        // Normalize movement direction
        moveDir = moveDir.normalized;
        // Move the player in the players forward direction based on the moveDir
        transform.position += (transform.forward * moveDir.y + transform.right * moveDir.x) * moveSpeed * Time.deltaTime;

        // Cursor Locking
        if (Input.GetKeyDown(KeyCode.Escape)) // Unlock the cursor if the escape key is pressed
        { 
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) // Lock the cursor if the left or right mouse button is pressed
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        // Only do rotation if the cursor is locked
        if (Cursor.lockState != CursorLockMode.Locked) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        rotationX -= mouseY; // X rotation is controlled by the mouse movement on the Y axis
        rotationY += mouseX; // Y rotation is controlled by the mouse movement on the X axis
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);  // Clamp the X rotation to prevent the camera from flipping
        
        // Apply the rotation to the player and camera 
        transform.localRotation = Quaternion.Euler(0, rotationY, 0);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        // Toggle flashlight
        if (Input.GetKeyDown(KeyCode.F))
        {
            flashlight.gameObject.SetActive(!flashlight.gameObject.activeSelf);
        }
    }
}