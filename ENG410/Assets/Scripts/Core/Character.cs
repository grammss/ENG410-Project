using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Character
{
  public string characterName;
  public RectTransform root;
  public Image characterImage;
  public GameObject obj;

  public void Say(string speech)
  {
    DialogueSystem.inst.Say(speech, characterName);
  }

  public void Move(int position)
  {
    CharacterSystem.inst.MoveCharacter(this, 0);
  }

  public Character(string _name)
  {
    GameObject prefab = Resources.Load("characters/Character[" + _name + "]") as GameObject;
    obj = Object.Instantiate(prefab, CharacterSystem.inst.characterPanel);

    root = obj.GetComponent<RectTransform>();
    characterName = _name;

    characterImage = obj.transform.Find("Portrait").GetComponent<Image>();
  }
}
