using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Story Data/Story Branch", fileName = "Branch")]
public class Branch : ScriptableObject
{
  [TextArea(3, 10)]
  public List<string> story;
  // possible branches
  public List<Branch> pBranches = new List<Branch>(3);
}
