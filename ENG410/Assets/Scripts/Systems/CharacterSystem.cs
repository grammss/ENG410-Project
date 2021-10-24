using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSystem : MonoBehaviour
{
  public static CharacterSystem inst;

  public RectTransform characterPanel;

  public List<Character> characters = new List<Character>();

  public Dictionary<string, int> characterDictionary = new Dictionary<string, int>();

  private void Awake()
  {
    inst = this;
  }

  public Character GetCharacter(string characterName, bool createCharacterIfDoesNotExist = true)
  {
    int index = -1;
    if (characterDictionary.TryGetValue(characterName, out index))
    {
      return characters[index];
    }
    else if (createCharacterIfDoesNotExist)
    {
      return CreateCharacter(characterName);
    }
    return null;
  }

  public Character CreateCharacter(string characterName)
  {
    Character newCharacter = new Character(characterName);

    characterDictionary.Add(characterName, characters.Count);
    characters.Add(newCharacter);

    return newCharacter;
  }
}
