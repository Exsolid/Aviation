using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Credits : MonoBehaviour
{
    VideoPlayer video;
    private bool isSpeeding;

    void Awake()
    {
        video = GetComponent<VideoPlayer>();
        video.Play();
        video.loopPointReached += CheckOver;
    }

    void CheckOver (UnityEngine.Video.VideoPlayer vp)
    {
        SceneManager.LoadScene("MenuMain");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            video.Pause();
            video.playbackSpeed =5f;
            video.Play();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            video.Pause();
            video.playbackSpeed = 1f;
            video.Play();
        }
        if(Input.GetKeyUp(KeyCode.Escape))
            SceneManager.LoadScene("MenuMain");
    }
}
