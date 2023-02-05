using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Characters/Character")]
public class Character : ScriptableObject
{
    [SerializeField] private int id = -1;
    [SerializeField] private string displayName = "Name";
    [SerializeField] private Sprite icon;

    // Public getters
    public int Id => id;
    public string DisplayName => displayName;
    public Sprite Icon => icon;
}