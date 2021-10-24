using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSystem : MonoBehaviour
{
  public int browsing = 0, playing = 0;
  public AudioSource[] BGMs, SFXs;
  bool changedBGM = false;

  public Text musicText;
  public string[] songNames;
  public List<int> bannedSongs;

  private void Awake()
  {
    StartCoroutine(AudioLoop());
    UpdateUI();
  }

  void UpdateUI()
  {
    if (bannedSongs.Contains(browsing))
      musicText.text = "Now Playing: " + songNames[playing] + "\n" + "Browsing: <color=red>" + songNames[browsing] + "</color>";
    else
      musicText.text = "Now Playing: " + songNames[playing] + "\n" + "Browsing: " + songNames[browsing];
  }
  public void BrowseNext()
  {
    ++browsing;
    if (browsing >= BGMs.Length)
      browsing -= BGMs.Length;
    UpdateUI();
  }
  public void BrowsePrev()
  {
    --browsing;
    if (browsing < 0)
      browsing += BGMs.Length;
    UpdateUI();
  }
  public void PlayBrowsing()
  {
    if (browsing == playing || bannedSongs.Contains(browsing))
      return;
    playing = browsing;
    changedBGM = true;
    UpdateUI();
  }
  public void PlaySFX(int index)
  {

  }

  IEnumerator AudioLoop()
  {
    while (true)
    {
      if (changedBGM)
      {
        for (float t = 0; t <= 1; t += Time.deltaTime * 4)
        {
          for (int i = 0; i < BGMs.Length; ++i)
            if (playing != i)
              BGMs[i].Stop();
          yield return null;
        }
        yield return null;
        for (float t = 0; t <= 1; t += Time.deltaTime * 2)
        {
          for (int i = 0; i < BGMs.Length; ++i)
            if (playing == i)
              BGMs[i].Play();
          yield return null;
        }
        changedBGM = false;
      }
      yield return null;
    }
  }
}
