using UnityEngine;

[CreateAssetMenu(menuName = "IRAI/Collectible")]
public class CollectibleData : ScriptableObject
{
    public string displayName;
    public Sprite icon;
    public float healAmount;
    public float feedAmount;          // hunger restore
    public float ahanFeedAmount;      // milk, etc.
    public float courageBonus;
    public int scoreValue = 25;
    public bool isMemoryShard;
    public AudioClip pickupSfx;
}