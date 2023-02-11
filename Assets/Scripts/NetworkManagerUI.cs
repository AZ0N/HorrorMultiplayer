using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private GameObject buttonParent; 
    [SerializeField] private Button hostButton;
    [SerializeField] private Button serverButton;
    [SerializeField] private Button clientButton;

    private bool isConnecting = false;

    private void Awake()
    {
        hostButton.onClick.AddListener(() => StartHost());
        clientButton.onClick.AddListener(() => StartClient());

        //TODO Determine if it's needed to make a server-instance
        serverButton.onClick.AddListener(() => {NetworkManager.Singleton.StartServer(); gameObject.SetActive(false);});
    }

    private void OnEnable()
    {
        //? Do they need to be removed in OnDisable()?
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
    }

    private void OnClientConnected(ulong clientId)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            return;
        }
        // We succesfully connected
        isConnecting = false;
    }

    private void OnClientDisconnected(ulong clientId)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            return;
        }

        // Differentiate between failed connection attempt and lost connection
        if (isConnecting)
        {
            // Reached on a failed connection attempt
            // TODO Handle
            Debug.Log("Connection attempt failed");
        }
        else
        {
            // Reached when disconnecting after being connected
            //TODO Reset scene and show menu on lost connection / Reconnect-button
            Debug.Log("Lost connection to server");
        }
        isConnecting = false;
        buttonParent.SetActive(true);
        NetworkManager.Singleton.Shutdown();
    }

    private void StartClient()
    {
        // Try to start client and connect to server
        isConnecting = true;
        buttonParent.SetActive(false);
        NetworkManager.Singleton.StartClient();
    }

    private void StartHost()
    {
        // Start host (server and connect to self as client)
        isConnecting = true;
        buttonParent.SetActive(false);
        NetworkManager.Singleton.StartHost();
    }
}
