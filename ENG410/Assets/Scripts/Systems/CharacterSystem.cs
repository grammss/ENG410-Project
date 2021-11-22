using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSystem : MonoBehaviour
{
  public static CharacterSystem inst;

  public RectTransform characterPanel;

  public List<Character> characters = new List<Character>();
  public List<CharacterPlaces> characterPlaces = new List<CharacterPlaces>();

  public CharacterPlaces currentPlaces;

  public Dictionary<string, int> characterDictionary = new Dictionary<string, int>();

  public float characterMoveSpeed;

  private void Awake()
  {
    inst = this;
    StartCoroutine(Loop());
  }

  public void MoveCharacter(Character character, int index, float speed = 1)
  {
    if (index < 0 || index > currentPlaces.places.Count)
      return;
    character.obj.transform.position = Vector3.MoveTowards(
      character.obj.transform.position,
      currentPlaces.places[index].transform.position,
      Time.deltaTime * characterMoveSpeed * speed);
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

  IEnumerator Loop()
  {
    while (true)
    {

      yield return null;
    }
  }
}
