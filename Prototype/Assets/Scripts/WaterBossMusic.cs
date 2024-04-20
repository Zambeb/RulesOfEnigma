using System.Collections;
using UnityEngine;

public class WaterBossMusic : MonoBehaviour
{
    public GameObject music;
    public GameObject bossMusic;
    public float fadeDuration = 2f;

    void Start()
    {
        music.SetActive(true);
        bossMusic.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(FadeOutAndSwitch());
        }
    }

    IEnumerator FadeOutAndSwitch()
    {
        bossMusic.SetActive(true);
        AudioSource musicAudioSource = music.GetComponent<AudioSource>();
        AudioSource bossMusicAudioSource = bossMusic.GetComponent<AudioSource>();

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float normalizedTime = elapsedTime / fadeDuration;
            musicAudioSource.volume = Mathf.Lerp(1f, 0f, normalizedTime);
            bossMusicAudioSource.volume = Mathf.Lerp(0f, 1f, normalizedTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        music.SetActive(false);
   
    }
}