using UnityEngine;

public static class CourageSystem
{
    public static void GrantCourage(PlayerStats s, float amt, string reason = "")
    {
        if (!s) return;
        s.AddCourage(amt);
        GameEvents.RaiseScoreDelta(Mathf.RoundToInt(amt));
    }

    public static bool CanTakeBraveChoice(PlayerStats s, float threshold = 40f)
    {
        return s && s.courage >= threshold;
    }
}