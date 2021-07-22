using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreamGameOver : MonoBehaviour
{

    void Awake ()
    {
        //Your code here
        var videoPlayer = GetComponent<UnityEngine.Video.VideoPlayer>();
        videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath,"Game Over.mp4");
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.Play();
    }
}
