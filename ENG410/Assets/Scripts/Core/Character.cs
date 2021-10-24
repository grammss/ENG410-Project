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

  public void Say(string speech)
  {
    DialogueSystem.inst.Say(speech, characterName);
  }

  public Character(string _name)
  {
    GameObject prefab = Resources.Load("characters/Character[" + _name + "]") as GameObject;
    GameObject obj = Object.Instantiate(prefab, CharacterSystem.inst.characterPanel);

    root = obj.GetComponent<RectTransform>();
    characterName = _name;

    characterImage = obj.transform.Find("Portrait").GetComponent<Image>();
  }
}
