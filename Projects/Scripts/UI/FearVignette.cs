using UnityEngine;
using UnityEngine.UI;

public class FearVignette : MonoBehaviour
{
    public Image vignetteImage;      // radial purple/black
    public float maxAlpha = 0.7f;

    float fear01, proximity01;

    void OnEnable()
    {
        GameEvents.OnFearChanged += (c, m) => fear01 = c / m;
        GameEvents.OnVeilProximity += v => proximity01 = v;
    }

    void Update()
    {
        if (!vignetteImage) return;
        float a = Mathf.Max(fear01 * 0.6f, proximity01 * maxAlpha);
        var c = vignetteImage.color; c.a = Mathf.Lerp(c.a, a, Time.deltaTime * 3f); vignetteImage.color = c;
    }
}