using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class LevelCutScenePlay : MonoBehaviour
{
    private VideoPlayer video;
    private Image fadeImage;

    void Awake()
    {
        video = GetComponentInChildren<VideoPlayer>();
        video.Play();
        video.loopPointReached += CheckOver;
        fadeImage = GetComponent<Image>();
    }

    void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        StartCoroutine(fade(false));
        video.enabled = false;
        AviationEventManagerMenu.Instance.CutSceneEnd();
        StartCoroutine(fade(true));
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && video.enabled)
        {
            video.Pause();
            video.playbackSpeed = 5f;
            video.Play();
        }
        else if (Input.GetKeyUp(KeyCode.Space) && video.enabled)
        {
            video.Pause();
            video.playbackSpeed = 1f;
            video.Play();
        }
        if (Input.GetKeyUp(KeyCode.Escape) && video.enabled)
        {
            video.Stop();
            CheckOver(video);
        }
    }

    IEnumerator fade(bool fadeAway)
    {
        if (fadeAway)
        {
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                fadeImage.color = new Color(0, 0, 0, i);
                yield return null;
            }
        }
        else
        {
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                fadeImage.color = new Color(0, 0, 0, i);
                yield return null;
            }
        }
    }
}
