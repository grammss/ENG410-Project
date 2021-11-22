using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Environment
{
  public string envName;
  public RectTransform root;
  public Image envImage;
  public GameObject obj;

  public Environment(string _name)
  {
    GameObject prefab = Resources.Load("environments/Environment[" + _name + "]") as GameObject;
    obj = Object.Instantiate(prefab, CharacterSystem.inst.characterPanel);

    root = obj.GetComponent<RectTransform>();
    envName = _name;

    envImage = obj.GetComponent<Image>();
  }
}
