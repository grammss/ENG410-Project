using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSystem : MonoBehaviour
{
  public static CharacterSystem inst;

  float defaultSpeed = 2;

  public RectTransform characterPanel;

  public List<Character> characters = new List<Character>();
  public List<CharacterPlaces> characterPlaces = new List<CharacterPlaces>();

  public CharacterPlaces currentPlaces;

  public Dictionary<string, int> characterDictionary = new Dictionary<string, int>();

  public float characterMoveSpeed;

  public Image char1, ribbon;
  Sprite targetTexture = null;
  Sprite[] chars;

  public float speed = 2;
  public bool fade = false;
  public bool fadeRibbon = false;

  private void Awake()
  {
    inst = this;
    StartCoroutine(Loop());
    LoadAllChars();
  }

  private void LoadAllChars()
  {
    chars = Resources.LoadAll<Sprite>("characters");
    foreach (Sprite s in chars)
    {
      characterDictionary.Add(s.name, characterDictionary.Count);
    }
  }

  public void ToggleRibbon(bool toggle)
  {
    fadeRibbon = (toggle);
  }

  public void ChangeCharacter(string _name, float _speed = 2)
  {
    speed = Mathf.Clamp(_speed, 1, 10);
    int index = -1;
    //if (characterDictionary.TryGetValue("Character[" + _name + "]", out index))
    if (characterDictionary.TryGetValue("" + _name + "", out index))
      targetTexture = chars[index];
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
      if (targetTexture)
      {
        if (!char1.sprite)
        {
          char1.sprite = targetTexture;
          for (float t = 0; t <= 1; t += Time.deltaTime * speed * defaultSpeed)
          {
            char1.color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, t);
            yield return null;
          }
        }
        char1.sprite = targetTexture;
        char1.color = Color.white;
        targetTexture = null;
      }
      if (fade)
      {
        for (float t = 0; t <= 1; t += Time.deltaTime * speed * defaultSpeed)
        {
          char1.color = Color.Lerp(Color.white, new Color(1, 1, 1, 0), t);
          yield return null;
        }
        char1.color = new Color(1, 1, 1, 0);
        fade = false;
      }
      if (fadeRibbon)
      {
        for (float t = 0; t <= 1; t += Time.deltaTime * speed * defaultSpeed)
        {
          ribbon.color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, t);
          yield return null;
        }
        ribbon.color = Color.white;
        fadeRibbon = false;
      }
      yield return null;
    }
  }
}
