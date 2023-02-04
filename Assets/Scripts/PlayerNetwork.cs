using UnityEngine;
using Unity.Netcode;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private AudioListener audioListener;


    private Vector2 moveDir;

    private void Start() {
        playerCamera.enabled = IsOwner;
        audioListener.enabled = IsOwner;
    }

    private void Update() {
        if (IsOwner) {
        moveDir = Vector2.zero;
            if (Input.GetKey(KeyCode.W)) moveDir.y += 1;
            if (Input.GetKey(KeyCode.S)) moveDir.y -= 1;
            if (Input.GetKey(KeyCode.A)) moveDir.x -= 1;
            if (Input.GetKey(KeyCode.D)) moveDir.x += 1;
        }
    }

    private void FixedUpdate() {
        if (!IsOwner) return;
        MovePlayerServerRpc(moveDir);
    }

    [ServerRpc]
    private void MovePlayerServerRpc(Vector2 moveDir, ServerRpcParams serverRpcParams = default) {
        // Normalize movement direction
        moveDir = moveDir.normalized;
        transform.position += new Vector3(moveDir.x, 0, moveDir.y) * moveSpeed * Time.fixedDeltaTime;
    }
}