using System.Collections;
using UnityEngine;

public class Scene5_NightCamp : MonoBehaviour
{
    public AudioClip campMusic;
    public GameObject campfire;
    public Transform[] shadowSpawnPoints;
    public GameObject shadowPrefab;

    public float nightDuration = 90f;        // seconds until dawn
    public int spawnEvery = 6;
    public int maxShadowsAlive = 4;

    float timer;
    int aliveShadows;

    void Start()
    {
        AudioManager.Instance?.PlayMusic(campMusic);
        StartCoroutine(SpawnLoop());
        StartCoroutine(NightClock());
    }

    IEnumerator NightClock()
    {
        while (timer < nightDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        UIManager.Instance?.Toast("Dawn breaks.");
        foreach (var s in FindObjectsOfType<ShadowEnemy>()) s.FadeAndDie();
        GameEvents.RaiseScoreDelta(200);
        yield return new WaitForSeconds(2f);
        SceneLoader.Instance.Load("06_FinalVeil");
    }

    IEnumerator SpawnLoop()
    {
        while (timer < nightDuration)
        {
            yield return new WaitForSeconds(spawnEvery);
            if (aliveShadows >= maxShadowsAlive) continue;
            if (!campfire || !campfire.activeSelf) continue;  // fire died = no enemies? Or more!
            var p = shadowSpawnPoints[Random.Range(0, shadowSpawnPoints.Length)];
            var s = Instantiate(shadowPrefab, p.position, Quaternion.identity);
            aliveShadows++;
        }
    }
}