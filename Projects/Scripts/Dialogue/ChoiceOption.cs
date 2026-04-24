using UnityEngine;

[System.Serializable]
public class ChoiceOption
{
    public string label;
    [TextArea(1, 3)] public string responseText;
    public int karmaDelta;          // -30..+30
    public float courageDelta;
    public float fearDelta;
    public int scoreDelta;
    public float minCourageRequired; // if >0, greyed out when courage below
}