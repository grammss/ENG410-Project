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
    AudioSystem.inst.BrowseNext();
    AudioSystem.inst.BrowsePrev();
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
    return speechCount >= currentBranch.story.Count && currentBranch.pBranches.Count != 0;
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
              FadeSystem.inst.FadeOutForeground();
              FadeSystem.inst.EndGame();
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
                  string[] sparts = speaker.Split('>');
                  CharacterSystem.inst.ChangeCharacter(sparts[1]);
                }
                // fade portrait
                else if (speaker[0] == '-')
                {
                  CharacterSystem.inst.fade = true;
                }
                // fade ribbon
                else if (speaker[0] == '+')
                {
                  CharacterSystem.inst.ToggleRibbon(true);
                }
                // cut the music
                else if (speaker[0] == '<')
                {
                  AudioSystem.inst.PlaySpecific(-1);
                }
                // girl hops
                else if (speaker[0] == '^')
                {
                  CharacterSystem.inst.hop = true;
                }
                // toggle boy
                else if (speaker[0] == '&')
                {
                  CharacterSystem.inst.boyOn = !CharacterSystem.inst.boyOn;
                }
                // shake
                else if (speaker[0] == '$')
                {
                  FadeSystem.inst.Shake();
                }
                // horror strings
                else if (speaker[0] == '0')
                {
                  AudioSystem.inst.PlaySFX(0);
                }
                // crunch
                else if (speaker[0] == '1')
                {
                  AudioSystem.inst.PlaySFX(1);
                }
                // thud
                else if (speaker[0] == '2')
                {
                  AudioSystem.inst.PlaySFX(2);
                }
                // rib item
                else if (speaker[0] == '3')
                {
                  AudioSystem.inst.PlaySFX(3);
                }
                // rib item
                else if (speaker[0] == '4')
                {
                  CharacterSystem.inst.rib.gameObject.SetActive(true);
                }
                // cat item
                else if (speaker[0] == '5')
                {
                  CharacterSystem.inst.cat.gameObject.SetActive(true);
                }
                // mp item
                else if (speaker[0] == '6')
                {
                  CharacterSystem.inst.mp.gameObject.SetActive(true);
                }
                // mp item
                else if (speaker[0] == '8')
                {
                  CharacterSystem.inst.rip.gameObject.SetActive(true);
                }
                // mp item
                else if (speaker[0] == '9')
                {
                  AudioSystem.inst.PlaySFX(4);
                }
                // hide item
                else if (speaker[0] == '7')
                {
                  CharacterSystem.inst.rib.gameObject.SetActive(false);
                  CharacterSystem.inst.cat.gameObject.SetActive(false);
                  CharacterSystem.inst.mp.gameObject.SetActive(false);
                  CharacterSystem.inst.rip.gameObject.SetActive(false);
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
            {
              if (currentBranch.pBranches.Count == 0)
              {
              }
              else
              {
                // Choose new BGM
                WaitNewBGM();
              }
            }
          }
        }
      }
      yield return null;
    }
  }
}
