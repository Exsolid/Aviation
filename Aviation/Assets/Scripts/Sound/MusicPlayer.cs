using AudioBuddyTool;
using System;
public class MusicPlayer
{
    private AudioBuddySpeaker speaker;
    public AudioBuddySpeaker Speaker { get { return speaker; } set { if (speaker != null) speaker.SourcePlayer.Stop(); speaker = value; } }
    private static MusicPlayer instance;
    public static MusicPlayer Instance { get { if (instance == null) instance = new MusicPlayer(); return instance; } }

    public MusicPlayer()
    {

    }
}
