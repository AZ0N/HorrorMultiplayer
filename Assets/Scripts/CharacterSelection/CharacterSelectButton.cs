using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectButton : MonoBehaviour
{
    [SerializeField] private Image iconImage;

    private CharacterSelectDisplay display;
    private Character character;

    public void SetCharacter(CharacterSelectDisplay display, Character character)
    {
        this.display = display;
        this.character = character;
        this.iconImage.sprite = character.Icon;
    }

    public void SelectCharacter()
    {
        display.SelectCharacter(character);
    }
}
