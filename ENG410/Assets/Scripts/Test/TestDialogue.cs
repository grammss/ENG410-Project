using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDialogue : MonoBehaviour
{
  public string[] s =
  {
    "Hello, I am Grammama",
    "I like big butts and I cannot lie",
    "LULW",
  };

  int index = 0;

  void Update()
  {
    //if (Input.GetKeyDown(KeyCode.Space))
    //{
    //  if (!DialogueSystem.inst.isSpeaking
    //    || DialogueSystem.inst.isWaitingForUserInput)
    //  {
    //    if (index >= s.Length)
    //    {
    //      return;
    //    }
    //    Say(s[index]);
    //    ++index;
    //  }
    //}
  }

  void Say(string s)
  {
    string[] parts = s.Split(':');
    string speech = parts[0];
    string speaker = (parts.Length > 1) ? parts[1] : "";

    DialogueSystem.inst.Say(speech, speaker);
  }
}
