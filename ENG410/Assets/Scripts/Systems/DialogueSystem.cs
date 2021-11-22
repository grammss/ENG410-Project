using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
  public static DialogueSystem inst;

  public Font sysFont, diaFont;
  public ELEMENTS elements;

  private void Awake()
  {
    inst = this;
  }
  private void Update()
  {
    //print(speaking);
  }

  /// <summary>
  /// Say something and show it on the speech box.
  /// </summary>
  public void Say(string speech, string speaker = "")
  {
    StopSpeaking();

    speaking = StartCoroutine(Speaking(speech, false, speaker));
  }

  /// <summary>
  /// Say something to be added to what is already on the speech box.
  /// </summary>
  public void SayAdd(string speech, string speaker = "")
  {
    StopSpeaking();

    speechText.text = targetSpeech;

    speaking = StartCoroutine(Speaking(speech, true, speaker));
  }
  public void SkipText()
  {
    speakingIndex = maxIndex;
  }
  public void StopSpeaking()
  {
    if (isSpeaking)
    {
      StopCoroutine(speaking);
    }
    speaking = null;
  }

  public bool isSpeaking { get { return speaking != null; } }
  [HideInInspector] public bool isWaitingForUserInput = false;

  public string targetSpeech = "";
  Coroutine speaking = null;
  int speakingIndex = 0;
  int maxIndex = 0;
  IEnumerator Speaking(string speech, bool additive, string speaker = "")
  {
    speakingIndex = 0;
    if (!speech.EndsWith(" "))
      speech += ' ';
    maxIndex = speech.Length;
    speechPanel.SetActive(true);
    targetSpeech = speech + "</color>";

    if (!additive)
      speechText.text = "";
    else
      targetSpeech = speechText.text + targetSpeech;

    speakerNameText.text = DetermineSpeaker(speaker);//temporary

    //isWaitingForUserInput = false;

    speechText.text = targetSpeech;
    for (; speakingIndex < speech.Length; speakingIndex++)
    {
      string tmpText = targetSpeech;

      tmpText = tmpText.Insert(speakingIndex, "<color=#00000000>");
      speechText.text = tmpText;
      yield return new WaitForEndOfFrame();
    }
    speechText.text = speech;

    //text finished
    //isWaitingForUserInput = true;
    //while (isWaitingForUserInput)
    //  yield return new WaitForEndOfFrame();

    StopSpeaking();
  }

  string DetermineSpeaker(string s)
  {
    speakerNamePanel.SetActive(true);
    string retVal = speakerNameText.text;//default return is the current name
    if (s != speakerNameText.text && s != "")
      retVal = (s.ToLower().Contains("narrator")) ? "" : s;
    if (s == "")
      speakerNamePanel.SetActive(false);
    speechText.font = speakerNamePanel.activeInHierarchy ? diaFont : sysFont;
    speechText.fontStyle = speakerNamePanel.activeInHierarchy ? FontStyle.Normal : FontStyle.Italic;
    return retVal;
  }

  public void Close()
  {
    StopSpeaking();
    speechPanel.SetActive(false);
  }

  [System.Serializable]
  public class ELEMENTS
  {
    /// <summary>
    /// The main panel containing all dialogue related elements on the UI
    /// </summary>
    public GameObject speechPanel;
    public GameObject speakerNamePanel;
    public Text speakerNameText;
    public Text speechText;
  }
  public GameObject speechPanel { get { return elements.speechPanel; } }
  public GameObject speakerNamePanel { get { return elements.speakerNamePanel; } }
  public Text speakerNameText { get { return elements.speakerNameText; } }
  public Text speechText { get { return elements.speechText; } }
}
