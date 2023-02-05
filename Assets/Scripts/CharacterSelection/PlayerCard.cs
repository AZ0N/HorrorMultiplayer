using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCard : MonoBehaviour
{
    [SerializeField] private GameObject visualObject;
    [SerializeField] private Image playerIcon;
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private TMP_Text characterNameText;

    public void UpdateCard(CharacterSelecState state, Character character)
    {
        if (state.characterId != -1)
        {
            //Set Values
            characterNameText.text = character.DisplayName;
            playerIcon.sprite = character.Icon;
            playerIcon.enabled = true;
        }
        else
        {
            playerIcon.enabled = false;
            characterNameText.text = "Selecting...";
        }

        playerNameText.text = $"Player {state.clientId}";
        visualObject.SetActive(true);
    }

    public void DisableDisplay()
    {
        visualObject.SetActive(false);
    }
}
