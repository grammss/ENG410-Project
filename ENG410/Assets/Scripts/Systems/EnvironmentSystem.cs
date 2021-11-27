using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnvironmentSystem : MonoBehaviour
{
  public static EnvironmentSystem inst;
  public float speed = 2;
  float defaultSpeed = 2;
  public Image envTexture;
  Sprite targetTexture = null;

  Sprite[] envs;

  public Dictionary<string, int> envDictionary = new Dictionary<string, int>();

  private void Awake()
  {
    inst = this;
    LoadAllEnvs();
    StartCoroutine(Loop());
  }

  private void LoadAllEnvs()
  {
    envs = Resources.LoadAll<Sprite>("environments");
    foreach (Sprite s in envs)
    {
      envDictionary.Add(s.name, envDictionary.Count);
    }
  }

  public void ChangeEnvironment(string _name, float _speed = 1)
  {
    speed = Mathf.Clamp(_speed, 1, 10);
    int index = -1;
    //if (envDictionary.TryGetValue("Environment[" + _name + "]", out index))
    if (envDictionary.TryGetValue("" + _name + "", out index))
      targetTexture = envs[index];
  }

  IEnumerator Loop()
  {
    while (true)
    {
      if (targetTexture)
      {
        if (envTexture.sprite && envTexture.sprite != targetTexture)
        {
          FadeSystem.inst.FadeOutBackground(speed * defaultSpeed);
          for (float t = 0; t <= 1; t += Time.deltaTime * speed * defaultSpeed)
          {
            envTexture.color = Color.Lerp(Color.white, Color.clear, t);
            yield return null;
          }
          envTexture.color = Color.clear;
        }
        FadeSystem.inst.FadeInBackground(speed * defaultSpeed);
        envTexture.sprite = targetTexture;
        for (float t = 0; t <= 1; t += Time.deltaTime * speed * defaultSpeed)
        {
          envTexture.color = Color.Lerp(Color.clear, Color.white, t);
          yield return null;
        }
        envTexture.color = Color.white;
        targetTexture = null;
      }
      yield return null;
    }
  }
}
