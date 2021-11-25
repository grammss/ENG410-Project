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

  private void Start()
  {
    FadeSystem.inst.FadeInForeground();
    UpdateBannedSongs();
    DisablePlayButton();
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

  public void EnablePlayButton()
  {
    playButton.interactable = true;
  }

  public void DisablePlayButton()
  {
    playButton.interactable = false;
  }

  public void UpdateBannedSongs()
  {
    AudioSystem.inst.bannedSongs = new List<int>();
    for (int i = 0; i < currentBranch.pBranches.Count; ++i)
      if (!currentBranch.pBranches[i])
        AudioSystem.inst.bannedSongs.Add(i);
  }

  public void ChoosePath()
  {
    currentBranch = currentBranch.pBranches[AudioSystem.inst.browsing];
    speechCount = 0;
    UpdateBannedSongs();
    DisablePlayButton();
    Progress();
  }

  bool progress = false;
  public void Progress()
  {
    progress = true;
  }

  public bool WaitingForSongChoice()
  {
    return speechCount >= currentBranch.story.Count;
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
          if (autoplayTime > 4)
          {
            autoplayTime -= 4;
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
              AudioSystem.inst.PlaySpecific(-1);
              FadeSystem.inst.FadeInForeground();
              FadeSystem.inst.EndGame();
            }
            else
            {
              // Choose new BGM
              //WaitNewBGM();
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
