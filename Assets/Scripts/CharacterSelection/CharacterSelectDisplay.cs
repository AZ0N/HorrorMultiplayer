using TMPro;
using UnityEngine;
using Unity.Netcode;
using System.Linq;

public class CharacterSelectDisplay : NetworkBehaviour 
{
    [Header("Character DB")]
    [SerializeField] private CharacterDatabase characterDatabase;

    [Header("UI References")]
    [SerializeField] private Transform characterButtonParent;
    [SerializeField] private TMP_Text selectedCharacterText;
    [SerializeField] private PlayerCard[] playerCards;

    [Header("Prefabs")]
    [SerializeField] private CharacterSelectButton characterButtonPrefab;
    [SerializeField] private GameObject playerPrefab; //TODO MOVE!

    private NetworkList<CharacterSelecState> characterStates;

    private void Awake()
    {
        characterStates = new NetworkList<CharacterSelecState>();
    }

    public override void OnNetworkSpawn()
    {
        if (IsClient)
        {
            Character[] characters = characterDatabase.GetCharacters();
            // Loop over each character and instantiate buttons
            for (int i = 0; i < characters.Length; i++)
            {
                CharacterSelectButton charButton = Instantiate(characterButtonPrefab, characterButtonParent);
                charButton.SetCharacter(this, characters[i]);   
            }
            characterStates.OnListChanged += onPlayerStateChanged;
        }
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += onClientConnect;
            NetworkManager.Singleton.OnClientDisconnectCallback += onClientDisconnect;
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsClient)
        {
            characterStates.OnListChanged -= onPlayerStateChanged;
        }
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= onClientConnect;
            NetworkManager.Singleton.OnClientDisconnectCallback -= onClientDisconnect;
        }
    }

    private void onClientConnect(ulong clientId)
    {
        characterStates.Add(new CharacterSelecState(clientId));
    }

    private void onClientDisconnect(ulong clientId)
    {
        for (int i = 0; i < characterStates.Count; i++)
        {
            if (characterStates[i].clientId == clientId)
            {
                characterStates.RemoveAt(i);
                break;
            }   
        }
    }

    public void SelectCharacter(Character character)
    {
        // Update UI
        selectedCharacterText.text = $"Character {character.DisplayName}";
        // Notify server
        SelectCharacterServerRpc(character.Id);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SelectCharacterServerRpc(int characterId, ServerRpcParams serverRpcParams = default)
    {
        for (int i = 0; i < characterStates.Count; i++)
        {
            if (characterStates[i].clientId == serverRpcParams.Receive.SenderClientId)
            {
                // Check that the character exists
                if (characterDatabase.ContainsCharacter(characterId))
                {
                    characterStates[i] = new CharacterSelecState(characterStates[i].clientId, characterId);
                }
                else
                {
                    //TODO Notify client that the selected character is invalid
                }
            }
        }
    }

    public void OnStartGame()
    {
        if (IsServer)
        {
            //TODO Do somewhere else!
            foreach (var playerId in NetworkManager.Singleton.ConnectedClientsIds)
            {
                //TODO Select prefab based on selected character!
                GameObject playerGO = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
                playerGO.GetComponent<NetworkObject>().SpawnAsPlayerObject(playerId);
            }
            //TODO Make a child-object holding the UI, so the whole gameObject isn't disabled
            GameStartClientRpc();
        }
    }

    [ClientRpc]
    private void GameStartClientRpc(ClientRpcParams clientRpcParams = default) {
            gameObject.SetActive(false);
    }

    private void onPlayerStateChanged(NetworkListEvent<CharacterSelecState> changeEvent)
    {
        for (int i = 0; i < playerCards.Length; i++)
        {
            if (characterStates.Count > i)
            {
                playerCards[i].UpdateCard(characterStates[i], characterDatabase.GetCharacter(characterStates[i].characterId));
            }
            else
            {
                playerCards[i].DisableDisplay();
            }
        }
    }
}
