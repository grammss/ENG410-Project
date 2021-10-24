using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCharacter : MonoBehaviour
{
  public Character sakura;

  // Start is called before the first frame update
  void Start()
  {
    sakura = CharacterSystem.inst.GetCharacter("Sakura");
  }

  public string[] speech;
  int i = 0;

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Space))
    {
      if (i < speech.Length)
      {
        sakura.Say(speech[i]);
      }
      else
      {
        //DialogueSystem.inst.Close();
      }
      ++i;
    }
  }
}
