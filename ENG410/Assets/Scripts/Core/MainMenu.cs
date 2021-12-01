using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
  public static MainMenu inst;

  private void Awake()
  {
    inst = this;
    VisualNovelSystem.inst = null;
  }

  private void Start()
  {
    FadeSystem.inst.FadeInForeground();
  }

  public void StartGame()
  {
    SceneManager.LoadScene("Demo");
  }
  public void EndGame()
  {
    SceneManager.LoadScene("MainMenu");
  }
}
