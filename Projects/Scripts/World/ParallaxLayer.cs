using UnityEngine;

[ExecuteAlways]
public class ParallaxLayer : MonoBehaviour
{
    [Tooltip("0 = static, 1 = moves with camera, <1 = slower (far), >1 = faster (near)")]
    public float parallaxFactor = 0.5f;
    public bool infiniteX = true;

    Transform cam;
    Vector3 lastCamPos;
    float textureUnitSizeX;

    void Start()
    {
        cam = Camera.main ? Camera.main.transform : null;
        if (!cam) return;
        lastCamPos = cam.position;
        var sr = GetComponent<SpriteRenderer>();
        if (sr && sr.sprite)
        {
            var tex = sr.sprite.texture;
            textureUnitSizeX = tex.width / sr.sprite.pixelsPerUnit * transform.localScale.x;
        }
    }

    void LateUpdate()
    {
        if (!cam) return;
        Vector3 delta = cam.position - lastCamPos;
        transform.position += new Vector3(delta.x * parallaxFactor, delta.y * parallaxFactor * 0.3f, 0);
        lastCamPos = cam.position;

        if (infiniteX && textureUnitSizeX > 0f)
        {
            float offX = cam.position.x - transform.position.x;
            if (Mathf.Abs(offX) >= textureUnitSizeX)
                transform.position = new Vector3(cam.position.x, transform.position.y, transform.position.z);
        }
    }
}