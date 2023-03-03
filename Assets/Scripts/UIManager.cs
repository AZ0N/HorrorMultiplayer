using Unity.Netcode;
using UnityEngine;
using TMPro;

public class UIManager : NetworkBehaviour
{
    [SerializeField] private TMP_Text playerCountText;
    private NetworkVariable<int> connectedPlayers = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private void FixedUpdate()
    {
        if (IsServer)
        {
            connectedPlayers.Value = NetworkManager.Singleton.ConnectedClients.Count;
        }
        // Show number of connected players when connected
        bool isConnected = NetworkManager.Singleton.IsConnectedClient || NetworkManager.Singleton.IsHost;
        playerCountText.text = isConnected ? "Players: " + connectedPlayers.Value : "Not Connected";
    }
}