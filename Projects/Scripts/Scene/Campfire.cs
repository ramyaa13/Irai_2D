using UnityEngine;

public class Campfire : MonoBehaviour
{
    public float fuel = 100f;
    public float burnPerSec = 1f;
    public GameObject flameVisual;

    void Update()
    {
        fuel -= burnPerSec * Time.deltaTime;
        if (flameVisual) flameVisual.SetActive(fuel > 0);
    }
    public void AddFuel(float v) { fuel = Mathf.Min(100, fuel + v); }
}