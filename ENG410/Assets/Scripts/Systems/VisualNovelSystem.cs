using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Handles all choices and the novel itself
public class VisualNovelSystem : MonoBehaviour
{
  public static VisualNovelSystem inst;

  public Branch currentBranch;
  public int speechCount;

  public bool waitingNewBGM = false;
  public Button playButton;

  private float autoplayTime = 0;
  [SerializeField]
  private bool autoplay = false;
  private int autospeed = 0;
  private int textlength = 0;
  [SerializeField]
  private Color autoOnColor, autoOffColor;

  public Button autoButton;
  public Button speedButton;
  public Image autoBtnOverlay;
  public Text speedText;

  private void Awake()
  {
    inst = this;
    StartCoroutine(Loop());
    Progress();
  }

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.Space))
      Progress();
  }

  public void ToggleAuto()
  {
    autoplay = !autoplay;
    speedButton.interactable = autoplay;
    autoBtnOverlay.color = autoplay ? autoOnColor : autoOffColor;
    autoplayTime = 0;
  }
  public void ToggleSpeed()
  {
    autospeed = (autospeed + 1) % 4;
    speedText.text = "x" + (autospeed + 1);
  }

  private void WaitNewBGM()
  {
    EnablePlayButton();
    waitingNewBGM = true;
  }

  private void EnablePlayButton()
  {
    playButton.interactable = true;
  }

  private void DisablePlayButton()
  {
    playButton.interactable = false;
  }

  public void ChoosePath()
  {
    currentBranch = currentBranch.pBranches[AudioSystem.inst.browsing];
    DisablePlayButton();
    speechCount = 0;
    Progress();
  }

  bool progress = false;
  public void Progress()
  {
    progress = true;
  }

  IEnumerator Loop()
  {
    while (true)
    {
      if (!DialogueSystem.inst.isSpeaking
        || DialogueSystem.inst.isWaitingForUserInput)
      {
        if (autoplay)
        {
          autoplayTime += Time.deltaTime * (autospeed + 1) * (1.0f / Mathf.Log10(textlength));
          if (autoplayTime > 2)
          {
            autoplayTime -= 2;
            Progress();
          }
        }
      }


      if (progress)
      {
        progress = false;
        if (DialogueSystem.inst.isSpeaking)
        {
          DialogueSystem.inst.SkipText();
        }
        else if (!DialogueSystem.inst.isSpeaking
          || DialogueSystem.inst.isWaitingForUserInput)
        {
          if (speechCount >= currentBranch.story.Count)
          {
            if (currentBranch.pBranches.Count == 0)
            {
              // End game, roll credits?
              MainMenu.inst.EndGame();
            }
            else
            {
              // Choose new BGM
              WaitNewBGM();
            }
          }
          else
          {
            bool skip = true;
            while (skip)
            {
              string[] parts = currentBranch.story[speechCount].Split(':');
              string speech = (parts.Length > 1) ? parts[1] : parts[0];
              string speaker = (parts.Length > 1) ? parts[0] : "";

              if (speaker.Length > 0)
              {
                // change environment
                if (speaker[0] == '*')
                {
                  string[] sparts = speaker.Split('*');
                  EnvironmentSystem.inst.ChangeEnvironment(sparts[1]);
                }
                // fadeout
                else if (speaker[0] == '{')
                {
                  FadeSystem.inst.FadeOutBackground();
                }
                // change portrait
                else if (speaker[0] == '>')
                {

                }
                // normal dialogue
                else
                {
                  DialogueSystem.inst.Say(speech, speaker);
                  skip = false;
                }
              }
              // normal dialogue
              else
              {
                DialogueSystem.inst.Say(speech, speaker);
                skip = false;
                textlength = speech.Length;
              }
              ++speechCount;
            }
            if (speechCount >= currentBranch.story.Count)
              WaitNewBGM();
          }
        }
      }
      yield return null;
    }
  }
}