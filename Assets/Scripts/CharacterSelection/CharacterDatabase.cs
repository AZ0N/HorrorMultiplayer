using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDatabase", menuName = "Characters/CharacterDatabase")]
public class CharacterDatabase : ScriptableObject
{
    //TODO Should we use a dictionary instead, so each character isn't defined by their id (as an index)?
    [SerializeField] private Character[] characters = new Character[0];

    public Character[] GetCharacters() => characters;

    public Character GetCharacter(int id)
    {
        foreach (Character c in characters)
        {
            if (c.Id == id) {
                return c;
            }
        }
        return null;
    }
}