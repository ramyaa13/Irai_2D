using UnityEngine;

[System.Serializable]
public class ChoiceOption
{
    public string choiceId;             // <-- ADD THIS — e.g. "guard_plead", "guard_bribe"
    public string label;
    [TextArea(1, 3)] public string responseText;
    public int karmaDelta;
    public float courageDelta;
    public float fearDelta;
    public int scoreDelta;
    public float minCourageRequired;
    public string requiredItemId;       //
}