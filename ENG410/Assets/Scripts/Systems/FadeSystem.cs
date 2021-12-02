using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeSystem : MonoBehaviour
{
  public static FadeSystem inst;
  public GameObject ss;
  public Image bg, mg, fg;
  public Text endtxt;
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
    fadeQueue.Enqueue(ToggleFgFader(true));
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
  public void Shake()
  {
    fadeQueue.Enqueue(ScreenShake());
  }
  IEnumerator ScreenShake()
  {
    fading = true;
    Vector3 p = ss.transform.localPosition;
    for (float t = 0; t <= 1; t += Time.deltaTime * 2)
    {
      ss.transform.localPosition = p + new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)) * (1-t) * 50;
      yield return null;
    }
    ss.transform.localPosition = p;
    fading = false;
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
    fading = true;
    if (endtxt)
    {
      for (float t = 0; t <= 1; t += Time.deltaTime * 2)
      {
        endtxt.color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, t);
        yield return null;
      }
      endtxt.color = Color.white;
      yield return new WaitForSeconds(2);
    }
    if (endtxt)
    {
      for (float t = 0; t <= 1; t += Time.deltaTime * 2)
      {
        endtxt.color = Color.Lerp(Color.white, new Color(1, 1, 1, 0), t);
        yield return null;
      }
      endtxt.color = new Color(1, 1, 1, 0);
    }
    fg.gameObject.SetActive(true);
    for (float t = 0; t <= 1; t += Time.deltaTime * 2)
    {
      fg.color = Color.Lerp(Color.clear, Color.black, t);
      yield return null;
    }
    fg.color = Color.black;
    fading = false;
    yield return new WaitForSeconds(1);
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
