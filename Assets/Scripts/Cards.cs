using UnityEngine;

[CreateAssetMenu(fileName ="New Cards", menuName = "Create new card set", order = 51)]
public class Cards: ScriptableObject
{
    [SerializeField] private Card[] _arrayOfCards;
    public Card[] ArrayOfCards => _arrayOfCards;
}
