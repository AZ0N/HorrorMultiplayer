using TMPro;
using UnityEngine;
using Unity.Netcode;

public class CharacterSelectDisplay : NetworkBehaviour 
{
    [SerializeField] private CharacterDatabase characterDatabase;
    [SerializeField] private Transform characterButtonParent;
    [SerializeField] private CharacterSelectButton characterButtonPrefab;

    [SerializeField] private PlayerCard[] playerCards;

    [SerializeField] private TMP_Text selectedCharacterText;

    private NetworkList<CharacterSelecState> characterStates;

    private void Awake()
    {
        characterStates = new NetworkList<CharacterSelecState>();
    }

    public override void OnNetworkSpawn()
    {
        if (IsClient)
        {
            var characters = characterDatabase.GetCharacters();

            // Loop over each character and instantiate buttons
            for (int i = 0; i < characters.Length; i++)
            {
                var charButton = Instantiate(characterButtonPrefab, characterButtonParent);
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
                // TODO Verify character id?
                characterStates[i] = new CharacterSelecState(characterStates[i].clientId, characterId);
            }
        }
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
