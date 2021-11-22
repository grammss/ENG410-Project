using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterPlaces
{
  public List<GameObject> places = new List<GameObject>();
  public GameObject obj;

  public CharacterPlaces(int index)
  {
    GameObject prefab = Resources.Load("places/Places[" + index + "]") as GameObject;
    obj = Object.Instantiate(prefab, CharacterSystem.inst.characterPanel);

    foreach (GameObject place in obj.GetComponentsInChildren<GameObject>())
    {
      places.Add(place);
    }
  }
}
