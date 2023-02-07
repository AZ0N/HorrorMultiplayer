using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDatabase", menuName = "Characters/CharacterDatabase")]
public class CharacterDatabase : ScriptableObject
{
    //TODO Should we use a dictionary instead, so each character isn't defined by their id (as an index)?
    [SerializeField] private Character[] characters = new Character[0];
    public Character[] GetCharacters() => characters;

    public Character GetCharacter(int id)
    {
        return characters.FirstOrDefault(c => c.Id == id);
    }

    public bool ContainsCharacter(int id)
    {
        return GetCharacter(id) != null;
    }
}