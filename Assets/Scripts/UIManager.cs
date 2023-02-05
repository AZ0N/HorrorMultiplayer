using Unity.Netcode;
using UnityEngine;
using TMPro;

public class UIManager : NetworkBehaviour 
{
    [SerializeField] private TMP_Text playerCountText;
    private NetworkVariable<int> connectedPlayers = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private void FixedUpdate()
    {
        playerCountText.text = "Players: " + connectedPlayers.Value;

        if (IsServer) {
            connectedPlayers.Value = NetworkManager.Singleton.ConnectedClients.Count;
        }
    }
}