using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeSystem : MonoBehaviour
{
  public static FadeSystem inst;
  public Image bg, mg, fg;
  [SerializeField]
  Queue<IEnumerator> fadeQueue = new Queue<IEnumerator>();
  static bool fading = false;
  public static bool CanProceed()
  { return !fading; }

  private void Awake()
  {
    inst = this;
    StartCoroutine(Loop());
  }
  public void FadeInBackground(float speed = 1)
  {
    speed = Mathf.Clamp(speed, 1, 10);
    fadeQueue.Enqueue(FadeIn(bg, speed));
  }
  public void FadeInMidground(float speed = 1)
  {
    speed = Mathf.Clamp(speed, 1, 10);
    fadeQueue.Enqueue(FadeIn(mg, speed));
  }
  public void FadeInForeground(float speed = 1)
  {
    speed = Mathf.Clamp(speed, 1, 10);
    fadeQueue.Enqueue(FadeIn(fg, speed));
    fadeQueue.Enqueue(ToggleFgFader(false));
  }
  public void FadeOutBackground(float speed = 1)
  {
    speed = Mathf.Clamp(speed, 1, 10);
    fadeQueue.Enqueue(FadeOut(bg, speed));
  }
  public void FadeOutMidground(float speed = 1)
  {
    speed = Mathf.Clamp(speed, 1, 10);
    fadeQueue.Enqueue(FadeOut(mg, speed));
  }
  public void FadeOutForeground(float speed = 1)
  {
    speed = Mathf.Clamp(speed, 1, 10);
    fadeQueue.Enqueue(ToggleFgFader(true));
    fadeQueue.Enqueue(FadeOut(fg, speed));
  }
  public void StartGame()
  {
    fadeQueue.Enqueue(LoadGame());
  }
  public void EndGame()
  {
    fadeQueue.Enqueue(LoadMenu());
  }
  IEnumerator FadeIn(Image img, float speed)
  {
    fading = true;
    for (float t = 0; t <= 1; t += Time.deltaTime * speed)
    {
      img.color = Color.Lerp(Color.black, Color.clear, t);
      yield return null;
    }
    img.color = Color.clear;
    fading = false;
  }
  IEnumerator FadeOut(Image img, float speed)
  {
    fading = true;
    for (float t = 0; t <= 1; t += Time.deltaTime * speed)
    {
      img.color = Color.Lerp(Color.clear, Color.black, t);
      yield return null;
    }
    img.color = Color.black;
    fading = false;
  }
  IEnumerator LoadGame()
  {
    SceneManager.LoadScene("Demo");
    yield return null;
  }
  IEnumerator LoadMenu()
  {
    SceneManager.LoadScene("MainMenu");
    yield return null;
  }
  IEnumerator ToggleFgFader(bool boolean)
  {
    fg.gameObject.SetActive(boolean);
    yield return null;
  }
  IEnumerator Loop()
  {
    while (true)
    {
      while (fadeQueue.Count > 0)
      {
        yield return new WaitUntil(CanProceed);
        yield return StartCoroutine(fadeQueue.Dequeue());
        yield return new WaitUntil(CanProceed);
      }
      yield return null;
    }
  }
}
