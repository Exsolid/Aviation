using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioBuddyTool;
public class MusicInit : MonoBehaviour
{
    [SerializeField] private string trackName;

    private void Start()
    {
        GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayer>().Speaker = AudioBuddy.Play(trackName, Options.Instance.MusicVolume);
    }
}
