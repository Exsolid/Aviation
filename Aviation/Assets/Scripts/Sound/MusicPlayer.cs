using AudioBuddyTool;
using UnityEngine;
using System;
public class MusicPlayer: MonoBehaviour
{
    private AudioBuddySpeaker speaker;
    public AudioBuddySpeaker Speaker { 
        get { return speaker; } 
        set {
            if (speaker == null) speaker = value;
            else if (!value.SourceSound.Equals(speaker.SourceSound))
            {
                speaker.SourcePlayer.Stop();
                speaker = value;
            }else if (value.SourceSound.Equals(speaker.SourceSound))
            {
                speaker.SourcePlayer.volume = Options.Instance.MusicVolume;
                value.SourcePlayer.Stop();
            }
        }
    }
    private void Awake()
    {
        int numMusicPlayers =  FindObjectsOfType<MusicPlayer>().Length;
        if (numMusicPlayers != 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(AudioBuddy.Manager);
            DontDestroyOnLoad(gameObject);
        }
    }
}
